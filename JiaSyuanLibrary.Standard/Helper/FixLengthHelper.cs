using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JiaSyuanLibrary.Standard.CustomAttribute;
using JiaSyuanLibrary.Standard.Extension;

namespace JiaSyuanLibrary.Standard.Helper
{
    /// <summary>
    /// FixLengthHelper
    /// </summary>
    public static class FixLengthHelper
    {
        /// <summary>
        /// Fix Length String Deserialize To Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixLengthString"></param>
        /// <returns></returns>
        public static T FixLengthDeserialize<T>(string fixLengthString)
        {
            Type type = typeof(T);
            var modelCustomAttribute = type.GetTypeInfo().GetCustomAttributes().FirstOrDefault() as FixLengthAttribute;
            T firstModel = Activator.CreateInstance<T>();
            if (CheckLength(fixLengthString, modelCustomAttribute?.TotalLength ?? 0))
            {
                var topPropertyInfos = GetPropertyInfosAndSort(type);
                int index = 0;
                foreach (var topPropertyInfo in topPropertyInfos)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(topPropertyInfo.PropertyType) &&
                        topPropertyInfo.PropertyType != typeof(string))
                    {
                        Console.WriteLine(topPropertyInfo.Name);
                    }
                    else
                    {
                        SetPropertyValue<T>(topPropertyInfo, fixLengthString, ref index, ref firstModel);
                    }
                }
            }
            else
            {
                throw new Exception("Input Incorrect");
            }
            return firstModel;
        }

        /// <summary>
        /// Object Serialize To Fix Length String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public static string FixLengthSerialize<T>(T inputModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check Length
        /// </summary>
        /// <param name="fixLengthString"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        private static bool CheckLength(string fixLengthString, int totalLength)
        {
            return LengthCount(fixLengthString) == totalLength;
        }

        /// <summary>
        /// String Length Count
        /// </summary>
        /// <param name="fixLengthString"></param>
        /// <returns></returns>
        private static int LengthCount(string fixLengthString)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(fixLengthString))
            {
                result = fixLengthString.Length;
                result += fixLengthString.ToCharArray().Count(t => Convert.ToInt32(t) > 255);
                return result;
            }

            return result;
        }

        /// <summary>
        /// Set Property Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <param name="fixLengthString"></param>
        /// <param name="index"></param>
        /// <param name="model"></param>
        private static void SetPropertyValue<T>(PropertyInfo propertyInfo, string fixLengthString, ref int index, ref T model)
        {
            if (!(propertyInfo.GetCustomAttributes().FirstOrDefault() is FixLengthAttribute attribute))
            {
                throw new Exception($"Property:{propertyInfo.Name} without FixLengthAttribute");
            }
            else
            {
                model.GetType().GetProperty(propertyInfo.Name)?.SetValue(model, StringToPropertyType(fixLengthString, propertyInfo.PropertyType, attribute, ref index));
            }
        }

        /// <summary>
        /// String To Property Type
        /// </summary>
        /// <param name="fixLengthString"></param>
        /// <param name="propertyType"></param>
        /// <param name="attribute">Property's FixLengthAttribute</param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static dynamic StringToPropertyType(string fixLengthString, Type propertyType, FixLengthAttribute attribute, ref int index)
        {
            string propertyValueString = fixLengthString.Substring(index, attribute.Length);
            index += attribute.Length;
            if (propertyType == typeof(string))
            {
                int propertyValueStringLength = LengthCount(propertyValueString);
                if (propertyValueStringLength > attribute.Length)//全形處理
                {
                    int differenceIndex = propertyValueStringLength - attribute.Length;
                    propertyValueString = propertyValueString.Substring(0, differenceIndex);
                    index -= differenceIndex;
                }
                return propertyValueString.Trim();
            }
            else if (propertyType == typeof(decimal?))
            {
                return fixLengthString.StringToDecimalNullable(attribute.DecimalPlaces);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get PropertyInfos And Sort Properties
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> GetPropertyInfosAndSort(Type type)
        {
            Type fixLengthAttributeType = typeof(FixLengthAttribute);
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => new
            {
                Property = x,
                Attribute = (FixLengthAttribute)Attribute.GetCustomAttribute(x, fixLengthAttributeType, true)
            })
                .OrderBy(x => x.Attribute?.Order ?? -1)
                .Select(x => x.Property);
        }
    }
}
