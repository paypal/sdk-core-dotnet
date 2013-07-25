using System;

namespace PayPal.Log
{
    /// <summary>
    /// Abstract base for the loggers
    /// </summary>
    internal abstract class BaseLogger
    {
        /// <summary>
        /// Type specified
        /// </summary>
        private Type typeGiven;

        // Logger enable flag
        private bool isLoggerEnabled;

        /// <summary>
        /// Gets and sets the given Type
        /// </summary>
        public Type GivenType 
        { 
            get 
            { 
                return this.typeGiven; 
            } 
            private set 
            { 
                this.typeGiven = value; 
            } 
        }
           
        /// <summary>
        /// Get logger enable flag
        /// </summary>
        public bool IsEnabled 
        { 
            get 
            { 
                return this.isLoggerEnabled; 
            } 
            set 
            { 
                this.isLoggerEnabled = value; 
            } 
        }

        /// <summary>
        /// Abstract base 'BaseLogger' contructor overload
        /// </summary>
        /// <param name="typeGiven"></param>
        public BaseLogger(Type typeGiven)
        {
            this.GivenType = typeGiven;
            this.IsEnabled = true;
        }              

        /// <summary>
        /// Virtual wrapper for log4net ILog IsDebugEnabled
        /// </summary>
        public virtual bool IsDebugEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Virtual wrapper for log4net ILog IsErrorEnabled
        /// </summary>
        public virtual bool IsErrorEnabled 
        { 
            get 
            { 
                return true; 
            } 
        }

        /// <summary>
        /// Virtual wrapper for log4net ILog IsInfoEnabled
        /// </summary>
        public virtual bool IsInfoEnabled 
        { 
            get 
            { 
                return true; 
            } 
        }

        /// <summary>
        /// Abstract wrapper for log4net ILog Debug
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public abstract void Debug(System.Exception exception, string messageFormat, params object[] args);

        /// <summary>
        /// Abstract wrapper for log4net ILog DebugFormat
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public abstract void DebugFormat(string message, params object[] arguments);

        /// <summary>
        /// Abstract wrapper for log4net ILog Error
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public abstract void Error(System.Exception exception, string messageFormat, params object[] args);

        /// <summary>
        /// Abstract flush for loggers
        /// </summary>
        public abstract void Flush();

        /// <summary>
        /// Abstract wrapper for log4net ILog InfoFormat
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public abstract void InfoFormat(string message, params object[] arguments);
    }
}
