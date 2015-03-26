using System;

namespace PayPal.Log
{
    /// <summary>
    /// Abstract base for the loggers
    /// </summary>
    public abstract class BaseLogger
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
        /// Virtual wrapper for IsDebugEnabled
        /// </summary>
        public virtual bool IsDebugEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Virtual wrapper for IsErrorEnabled
        /// </summary>
        public virtual bool IsErrorEnabled 
        { 
            get 
            { 
                return true; 
            } 
        }

        /// <summary>
        /// Virtual wrapper for IsInfoEnabled
        /// </summary>
        public virtual bool IsInfoEnabled 
        { 
            get 
            { 
                return true; 
            } 
        }

        /// <summary>
        /// Virtual wrapper for IsWarnEnabled
        /// </summary>
        public virtual bool IsWarnEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Abstract wrapper for Debug
        /// </summary>
        /// <param name="message"></param>
        public abstract void Debug(string message);

        /// <summary>
        /// Abstract wrapper for Debug
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public abstract void Debug(string message, System.Exception exception);

        /// <summary>
        /// Abstract wrapper for DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public abstract void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Abstract wrapper for Error
        /// </summary>
        /// <param name="message"></param>
        public abstract void Error(string message);

        /// <summary>
        /// Abstract wrapper for Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public abstract void Error(string message, System.Exception exception);

        /// <summary>
        /// Abstract wrapper for ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public abstract void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Abstract wrapper for Info
        /// </summary>
        /// <param name="message"></param>
        public abstract void Info(string message);

        /// <summary>
        /// Abstract wrapper for Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public abstract void Info(string message, System.Exception exception);

        /// <summary>
        /// Abstract wrapper for InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public abstract void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Abstract wrapper for Warn
        /// </summary>
        /// <param name="message"></param>
        public abstract void Warn(string message);

        /// <summary>
        /// Abstract wrapper for Warn
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public abstract void Warn(string message, System.Exception exception);

        /// <summary>
        /// Abstract wrapper for WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public abstract void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Abstract flush for loggers
        /// </summary>
        public abstract void Flush();
    }
}
