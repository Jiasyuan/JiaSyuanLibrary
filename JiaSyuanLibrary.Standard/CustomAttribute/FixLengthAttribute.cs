using System;

namespace JiaSyuanLibrary.Standard.CustomAttribute
{
    /// <summary>
    /// Fix Length Attribute
    /// </summary>
    public class FixLengthAttribute : Attribute
    {
        /// <summary>
        /// Total Length
        /// </summary>
        public int TotalLength { get; set; }

        /// <summary>
        /// Fix Length Order
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Length
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Repeat Times
        /// </summary>
        public int RepeatTimes { get; set; } = 0;

        /// <summary>
        /// Is Pad Left
        /// </summary>
        public bool IsPadLeft { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public char PadChar { get; set; } = ' ';

        /// <summary>
        /// Decimal places
        /// </summary>
        public int DecimalPlaces { get; set; }
        /// <summary>
        /// DateTime Format
        /// </summary>
        public string DateTimeFormat { get; set; } = "yyyy/MM/dd HH:mm:ss";

    }
}
