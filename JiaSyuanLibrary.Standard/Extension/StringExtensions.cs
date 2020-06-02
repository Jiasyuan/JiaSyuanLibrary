using System;
using System.Collections.Generic;
using System.Linq;

namespace JiaSyuanLibrary.Standard.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert String To Int Nullable
        /// </summary>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static int? StringToIntNullable(this string inPut)
        {
            if (int.TryParse(inPut, out int result))
                return result;
            return null;
        }

        /// <summary>
        /// Convert String To decimal Nullable
        /// </summary>
        /// <param name="inPut"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static decimal? StringToDecimalNullable(this string inPut, int decimalPlaces)
        {
            if (string.IsNullOrWhiteSpace(inPut))
            {
                return null;
            }
            else
            {
                inPut = inPut.Insert(inPut.Length - decimalPlaces, ".");
                if (decimal.TryParse(inPut, out decimal result))
                    return result;
                return null;
            }
        }

        /// <summary>
        /// Order by traditional Chinese strokes
        /// </summary>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static IEnumerable<string> StringOrderbyStrokes(IEnumerable<string> inPut)
        {
            return inPut.OrderBy(o => o, StringComparer.Create(
                new System.Globalization.CultureInfo("zh-TW"), false)).ToList();
        }

        /// <summary>
        /// Order by Zhuyin
        /// </summary>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static IEnumerable<string> StringOrderbyZhuyin(IEnumerable<string> inPut)
        {
            return inPut.OrderBy(o => o, StringComparer.Create(
                new System.Globalization.CultureInfo(0x00030404), false)).ToList();
        }
    }
}
