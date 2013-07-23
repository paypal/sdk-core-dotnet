using System;

namespace PayPal.Log
{
    /// <summary>
    /// Comma separate the log types to enable multiple loggers
    /// </summary>
    [Flags]
    public enum LoggerTypes
    {
        /// <summary>
        /// Turn off log
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Log using log4net
        /// </summary>
        LOG4NET = 1,

        /// <summary>
        /// Log using System.Diagnostics
        /// </summary>
        DIAGNOSTICS = 2
    }
}  
