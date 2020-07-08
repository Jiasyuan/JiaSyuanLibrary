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
        /// Conver decimal  to  different formal string
        /// </summary>
        /// <param name="value">decimal value</param>
        /// <param name="format"></param>
        /// <param name="decimalNumber">保留的小數位數</param>
        /// <returns></returns>
        public static string ToString(this decimal value, CurrencyFormat format, int decimalNumber = 2)
        {
            switch (format)
            {
                case CurrencyFormat.Chinese:
                    //ref http://blog.darkthread.net/post-2009-12-23-chinese-number-char.aspx
                    var t = EastAsiaNumericFormatter.FormatWithCulture("L", value, null, _twCulture);
                    //修正EastAsiaNumericFormatter.FormatWithCulture出現"三百十"之問題，修正為三百一十的慣用寫法
                    var res = _regFixOne.Replace(t, m => "壹");
                    //拾萬需補為壹拾萬
                    if (res.StartsWith("拾")) res = "壹" + res;
                    return res;
                case CurrencyFormat.Comma:
                    return Math.Truncate(value).ToString("N");  //去小數
                case CurrencyFormat.CommaReservedDecimalNumber:
                    return Math.Round(value, decimalNumber).ToString($"N{ decimalNumber}");
                default:
                    return value.ToString();
            }
        }
    }
}
