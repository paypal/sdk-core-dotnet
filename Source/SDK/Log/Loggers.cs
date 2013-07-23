using System;

namespace PayPal.Log
{
    /// <summary>
    /// Comma separate the log types to enable multiple loggers
    /// </summary>
    [Flags]
    public enum Loggers
    {
        /// <summary>
        /// No log
        /// </summary>
        None = 0,

        /// <summary>
        /// Log using log4net
        /// </summary>
        Log4net = 1,

        /// <summary>
        /// Log using System.Diagnostics
        /// </summary>
        Diagnostics = 2
    }
}  
