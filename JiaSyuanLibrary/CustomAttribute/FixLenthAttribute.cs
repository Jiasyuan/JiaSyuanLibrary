using System;

namespace JiaSyuanLibrary.CustomAttribute
{
    /// <summary>
    /// Fix Lenth Attribute
    /// </summary>
    public class FixLenthAttribute: Attribute
    {
        /// <summary>
        /// Total Lenth
        /// </summary>
        public int TotalLenth { get; set; }

        /// <summary>
        /// Fix Lenth Order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        public int Length{ get; set; }

        /// <summary>
        /// Repeat Times
        /// </summary>
        public int RepeatTimes { get; set; }

        /// <summary>
        /// Is Pad Left
        /// </summary>
        public bool IsPadLeft { get; set; } = true;

    }
}
