using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiaSyuanLibrary.CustomAttribute;

namespace JiaSyuanLibrary.Helper
{
    /// <summary>
    /// FixLenthHelper
    /// </summary>
    public static class FixLenthHelper
    {
        /// <summary>
        /// Fix Lenth String Deserialize To Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FixLenthString"></param>
        /// <returns></returns>
        public static T FixLenthDeserialize<T>(string FixLenthString)
        {
            if (CheckLenth(FixLenthString))
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
        /// Object Serialize To Fix Lenth String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="InputModel"></param>
        /// <returns></returns>
        public static string FixLenthSerialize<T>(T InputModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FixLenthString"></param>
        /// <returns></returns>
        private static bool CheckLenth(string FixLenthString)
        {
            throw new NotImplementedException();
        }
    }
}
