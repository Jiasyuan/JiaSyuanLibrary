using System;
using System.Globalization;
using System.Text.RegularExpressions;
using JiaSyuanLibrary.Enums;
using Microsoft.International.Formatters;


namespace JiaSyuanLibrary.Helper
{
    public static class CurrencyHelper
    {
        private static readonly CultureInfo _twCulture = new CultureInfo("zh-TW");
        private static Regex _regFixOne = new Regex("(?<=[^壹貳參肆伍陸柒捌玖])(?=拾)");

        public static string ToString(this int value, CurrencyFormat format)
        {
            return ToString((decimal)value, format);
        }

        public static string ToString(this long value, CurrencyFormat format)
        {
            return ToString((decimal)value, format);
        }

        /// <summary>
        /// decimal ToString
        /// </summary>
        /// <param name="value"> decimal value</param>
        /// <param name="format">Currency Format</param>
        /// <param name="deciamlPlaces">Reserved deciaml Places</param>
        /// <returns></returns>
        public static string ToString(this decimal value, CurrencyFormat format, int deciamlPlaces = 2)
        {
            switch (format)
            {
                case CurrencyFormat.Chinese:
                    //ref http://blog.darkthread.net/post-2009-12-23-chinese-number-char.aspx
                    var t = EastAsiaNumericFormatter.FormatWithCulture("L", value, null, _twCulture);
                    var res = _regFixOne.Replace(t, m => "壹");
                    //拾萬需補為壹拾萬
                    if (res.StartsWith("拾")) res = "壹" + res;
                    return res;
                case CurrencyFormat.Comma:
                    return Math.Truncate(value).ToString("N");  //去小數
                case CurrencyFormat.CommaDeciamlPlaces:
                    return Math.Round(value, deciamlPlaces).ToString($"N{deciamlPlaces}");
                default:
                    return value.ToString();
            }
        }
    }
}
