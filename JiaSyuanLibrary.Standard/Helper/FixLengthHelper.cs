using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JiaSyuanLibrary.Standard.CustomAttribute;

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
            TypeInfo typeInfo = typeof(T).GetTypeInfo();
            var modelCustomAttribute = typeInfo.GetCustomAttributes().FirstOrDefault() as FixLengthAttribute;
            int totalLength = modelCustomAttribute?.TotalLength ?? 0;
            if (CheckLength(fixLengthString, totalLength))
            {
                //TODO:FixLenthDeserialize
            }
            else
            {
                throw new Exception("Input Incorrect");
            }
            throw new NotImplementedException();
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
        /// 
        /// </summary>
        /// <param name="fixLengthString"></param>
        /// <returns></returns>
        private static bool CheckLength(string fixLengthString, int totalLength)
        {
            throw new NotImplementedException();
        }
    }
}
