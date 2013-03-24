using System;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    /// <summary>
    /// Apply to classes or interfaces that represent data to be sent down to client
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PayloadAttribute : Attribute
    {
        public PayloadAttribute()
        {
            RoundNumbersTo = CompressionSettings.Default.RoundNumbersTo;
        }

        /// <summary>
        /// Determines how numbers are compressed via rounding.  
        /// Default is to not remove any digits after the decimal point (-1).
        /// </summary>
        public int RoundNumbersTo { get; set; }
    }
}
