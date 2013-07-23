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
        private Type typeSpecifying;

        // Flag for logger
        private bool isLoggerEnabled;

        /// <summary>
        /// Specifying Type
        /// </summary>
        public Type SpecifyingType 
        { 
            get 
            { 
                return this.typeSpecifying; 
            } 
            private set 
            { 
                this.typeSpecifying = value; 
            } 
        }
           
        /// <summary>
        /// Logger enable flag
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
        /// <param name="typeOfBase"></param>
        public BaseLogger(Type typeOfBase)
        {
            SpecifyingType = typeOfBase;
            IsEnabled = true;
        }

        /// <summary>
        /// Flush the loggers
        /// </summary>
        public abstract void Flush();

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
        /// Abstract wrapper for log4net ILog InfoFormat
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public abstract void InfoFormat(string message, params object[] arguments);
    }
}
