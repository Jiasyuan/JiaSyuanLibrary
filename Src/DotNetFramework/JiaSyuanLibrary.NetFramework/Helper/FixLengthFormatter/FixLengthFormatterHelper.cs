using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JiaSyuanLibrary.NetFramework.Helper.FixLengthFormatter.Attribute;

namespace JiaSyuanLibrary.NetFramework.Helper.FixLengthFormatter
{

    public static class FixLengthFormatterHelper
    {
        public static string Serialize<T>(T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // 取得所有帶 Attribute 的屬性並依 Order 排序
            var props = typeof(T).GetProperties()
                .Select(p => (Property: p, Attr: p.GetCustomAttribute<FixLengthAttribute>()))
                .Where(x => x.Attr != null)
                .OrderBy(x => x.Attr.Order)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var (property, attribute) in props)
            {
                var rawValue = property.GetValue(model);
                // 先轉字串，依顯示寬度截長或補齊
                var segment = FormatAndPad(rawValue, attribute);
                sb.Append(segment);
            }

            return sb.ToString();
        }

        public static T Deserialize<T>(string input) where T : new()
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            // 取得所有帶 Attribute 的屬性並依 Order 排序

            var props = typeof(T).GetProperties()
                .Select(p => (Property: p, Attr: p.GetCustomAttribute<FixLengthAttribute>()))
                .Where(x => x.Attr != null)
                .OrderBy(x => x.Attr.Order)
                .ToList();


            // 拆出每一段原始字串（依寬度）
            var widths = props.Select(x => x.Attr.Length).ToArray();
            var segments = SplitByDisplayWidth(input, widths);

            if (segments.Count != props.Count)
                throw new ArgumentException("欄位數量與屬性定義不符");


            var instance = new T();

            for (var i = 0; i < props.Count; i++)
            {
                var (property, attribute) = props[i];
                var raw = segments[i];
                var value = ParseValue(raw, property.PropertyType, attribute);
                property.SetValue(instance, value);
            }

            return instance;
        }

        /// <summary>
        /// 組合「截斷 + 補齊」
        /// </summary>
        /// <param name="value"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private static string FormatAndPad(object value, FixLengthAttribute attr)
        {
            // 原始字串
            string text;

            switch (value)
            {
                case null:
                    text = string.Empty;
                    break;
                case decimal d:
                    text = attr.DecimalPlaces != 0 ? 
                        DecimalToFixedPointString(d, attr.DecimalPlaces) : d.ToString($"F{attr.DecimalPlaces}");
                    break;
                case DateTime dt:
                    text = dt.ToString(attr.DateTimeFormat);
                    break;
                default:
                    text = value.ToString();
                    break;
            }

            // 先按寬度截長
            var trimmed = TruncateByWidth(text, attr.Length);

            // 再按寬度補齊
            return attr.IsPadLeft
                ? PadLeftByWidth(trimmed, attr.Length, attr.PadChar)
                : PadRightByWidth(trimmed, attr.Length, attr.PadChar);
        }

        /// <summary>
        /// 按寬度截長
        /// </summary>
        /// <param name="str">字串</param>
        /// <param name="maxWidth">最大寬度</param>
        /// <returns></returns>
        private static string TruncateByWidth(string str, int maxWidth)
        {
            var sb = new StringBuilder();
            int acc = 0;
            foreach (var c in str)
            {
                int w = GetCharDisplayWidth(c);
                if (acc + w > maxWidth) break;
                sb.Append(c);
                acc += w;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 按寬度左補齊
        /// </summary>
        /// <param name="s"></param>
        /// <param name="totalWidth"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        private static string PadLeftByWidth(string s, int totalWidth, char padChar)
        {
            int current = s.Sum(GetCharDisplayWidth);
            int needed = totalWidth - current;
            if (needed <= 0) return s;
            return new string(padChar, needed) + s;
        }

        /// <summary>
        /// 按寬度右補齊
        /// </summary>
        /// <param name="s"></param>
        /// <param name="totalWidth"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        private static string PadRightByWidth(string s, int totalWidth, char padChar)
        {
            int current = s.Sum(GetCharDisplayWidth);
            int needed = totalWidth - current;
            if (needed <= 0) return s;
            return s + new string(padChar, needed);
        }

        /// <summary>
        /// 依寬度拆分字串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="widths"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static List<string> SplitByDisplayWidth(string input, int[] widths)
        {
            var result = new List<string>();
            int charIndex = 0;

            foreach (var targetWidth in widths)
            {
                var sb = new StringBuilder();
                var acc = 0;
                while (charIndex < input.Length && acc < targetWidth)
                {
                    var c = input[charIndex++];
                    var w = GetCharDisplayWidth(c);
                    sb.Append(c);
                    acc += w;
                }

                if (acc != targetWidth)
                    throw new ArgumentException($"第 {result.Count + 1} 段長度錯誤（期望 {targetWidth}，實際 {acc}）。");

                result.Add(sb.ToString());
            }

            return result;
        }

        /// <summary>
        /// 解析回原生型別
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="targetType"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private static object ParseValue(string raw, Type targetType, FixLengthAttribute attr)
        {
            var trimmed = raw.Trim();

            if (targetType == typeof(string))
                return trimmed;

            if (targetType == typeof(int))
                return int.TryParse(trimmed, out var i) ? i : 0;

            if (targetType == typeof(decimal))
            {
                if (attr.DecimalPlaces != 0)
                    return ParseDecimalWithoutPoint(trimmed, attr.DecimalPlaces);
                return decimal.TryParse(trimmed, out var d) ? d : 0m;
            }

            if (targetType == typeof(DateTime))
                return DateTime.TryParseExact(
                    trimmed,
                    attr.DateTimeFormat,
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out var dt)
                        ? dt
                        : default;


            // bool（支援多種格式）
            if (targetType == typeof(bool))
            {
                if (bool.TryParse(trimmed, out var boolVal))
                    return boolVal;
                if (trimmed == "1") return true;
                if (trimmed == "0") return false;
                if (trimmed.Equals("Y", StringComparison.OrdinalIgnoreCase)) return true;
                if (trimmed.Equals("N", StringComparison.OrdinalIgnoreCase)) return false;
                if (trimmed == "是") return true;
                if (trimmed == "否") return false;
                return false;
            }

            // float
            if (targetType == typeof(float))
                return float.TryParse(trimmed, out var flt) ? flt : 0f;

            // double
            if (targetType == typeof(double))
                return double.TryParse(trimmed, out var dbl) ? dbl : 0d;

            // long
            if (targetType == typeof(long))
                return long.TryParse(trimmed, out var l) ? l : 0L;

            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                if (string.IsNullOrWhiteSpace(trimmed)) return null;
                return ParseValue(trimmed, Nullable.GetUnderlyingType(targetType), attr);
            }

            if (targetType == typeof(Guid))
                return Guid.TryParse(trimmed, out var guidVal) ? guidVal : Guid.Empty;

            if (targetType.IsEnum)
            {
                if (int.TryParse(trimmed, out var enumInt))
                    return Enum.ToObject(targetType, enumInt);
                return Enum.Parse(targetType, trimmed, ignoreCase: true);
            }


            // 其他型別 → 嘗試 Convert
            try
            {
                return Convert.ChangeType(trimmed, targetType);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 計算單一字元的「寬度」
        /// 全形 (East Asian Wide/Fullwidth) = 2
        /// 其他 (Halfwidth) = 1
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static int GetCharDisplayWidth(char c)
        {
            // 常見 CJK 全形字元區段範例
            if (
                (c >= '\u1100' && c <= '\u115F') ||
                (c >= '\u2E80' && c <= '\uA4CF') ||
                (c >= '\uAC00' && c <= '\uD7A3') ||
                (c >= '\uF900' && c <= '\uFAFF') ||
                (c >= '\uFE10' && c <= '\uFE19') ||
                (c >= '\uFE30' && c <= '\uFE6F') ||
                (c >= '\uFF00' && c <= '\uFF60') ||
                (c >= '\uFFE0' && c <= '\uFFE6'))
            {
                return 2;
            }

            return 1;
        }

        /// <summary>
        /// 將 decimal 轉換成不帶小數點的固定長度字串
        /// </summary>
        /// <param name="value">要轉換的數值</param>
        /// <param name="decimalPlaces">小數位數</param>
        /// <returns>不帶小數點的數字字串</returns>
        private static string DecimalToFixedPointString(decimal value, int decimalPlaces)
        {
            // 去掉小數點，產生純數字
            string raw = (value * (decimal)Math.Pow(10, decimalPlaces))
                .ToString($"F0", System.Globalization.CultureInfo.InvariantCulture);

            return raw;
        }



        private static decimal ParseDecimalWithoutPoint(string raw, int decimalPlaces)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return 0;
            raw = raw.Trim();
            // 確保字串長度足夠
            if (raw.Length <= decimalPlaces)
                raw = raw.PadLeft(decimalPlaces + 1, '0');

            // 插入小數點的位置
            var insertPos = raw.Length - decimalPlaces;
            var formatted = raw.Insert(insertPos, ".");

            return decimal.Parse(formatted);
        }
    }
}
