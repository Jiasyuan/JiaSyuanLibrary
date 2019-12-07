using System;

namespace JiaSyuanLibrary.Standard.CustomAttribute
{
    /// <summary>
    /// Fix Length Attribute
    /// </summary>
    public class FixLengthAttribute: Attribute
    {
        /// <summary>
        /// Total Length
        /// </summary>
        public int TotalLength { get; set; }

        /// <summary>
        /// Fix Length Order
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
