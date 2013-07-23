using System;
using System.Reflection;
using System.Globalization;

namespace PayPal.Log
{
    /// <summary>
    /// Wrapper for reflected log4net logging methods
    /// </summary>
    internal class Log4netReflection : BaseLogger
    {
        enum LoadState { Uninitialized, Failed, Loading, Success };
        
        static LoadState loadState = LoadState.Uninitialized;
        static readonly object LOCK = new object();
                
        static Type logMangerType;
        static MethodInfo loggerMethodInfo;

        static Type logType;
        static MethodInfo logMethod;

        static Type levelType;
        static object debugLevelPropertyValue;
        static object infoLevelPropertyValue;
        static object errorLevelPropertyValue;

        static MethodInfo isEnabledForMethod;
        static Type systemStringFormatType;
        static Type loggerType;

        private object internalLogger;
        private bool? isErrorEnabled;
        private bool? isDebugEnabled;
        private bool? isInfoEnabled;

        /// <summary>
        /// This should be a one time call to use reflection to find all the types and methods needed for the logging API.
        /// </summary>
        private static void Reflect()
        {
            lock (Log4netReflection.LOCK)
            {
                if (loadState != LoadState.Uninitialized)
                {
                    return;
                }

                loadState = LoadState.Loading;
                try
                {
                    loggerType = Type.GetType("PayPal.Log.Log4netWrapper");

                    logMangerType = Type.GetType("log4net.Core.LoggerManager, log4net");
                    if (logMangerType == null)
                    {
                        loadState = LoadState.Failed;
                        return;
                    }

                    loggerMethodInfo = logMangerType.GetMethod("GetLogger", new Type[] { typeof(Assembly), typeof(Type) });

                    logType = Type.GetType("log4net.Core.ILogger, log4net");
                    levelType = Type.GetType("log4net.Core.Level, log4net");
                    debugLevelPropertyValue = levelType.GetField("Debug").GetValue(null);
                    infoLevelPropertyValue = levelType.GetField("Info").GetValue(null);
                    errorLevelPropertyValue = levelType.GetField("Error").GetValue(null);

                    systemStringFormatType = Type.GetType("log4net.Util.SystemStringFormat, log4net");

                    logMethod = logType.GetMethod("Log", new Type[] { typeof(Type), levelType, typeof(object), typeof(System.Exception) });
                    isEnabledForMethod = logType.GetMethod("IsEnabledFor", new Type[] { levelType });

                    if (loggerMethodInfo == null ||
                        isEnabledForMethod == null ||
                        logType == null ||
                        levelType == null ||
                        logMethod == null)
                    {
                        loadState = LoadState.Failed;
                        return;
                    }

                    if ((LogConfiguration.Logging & Loggers.Log4net) == Loggers.Log4net)
                    {
                        Type xmlConfiguratorType = Type.GetType("log4net.Config.XmlConfigurator, log4net");
                        if (xmlConfiguratorType != null)
                        {
                            MethodInfo configureMethod = xmlConfiguratorType.GetMethod("Configure", Type.EmptyTypes);
                            if (configureMethod != null)
                            {
                                configureMethod.Invoke(null, null);
                            }
                        }
                    }

                    loadState = LoadState.Success;
                }
                catch
                {
                    loadState = LoadState.Failed;
                }
            }
        }

        public Log4netReflection(Type declaringType) : base(declaringType)
        {
            if (loadState == LoadState.Uninitialized)
            {
                Reflect();
            }

            if (logMangerType == null)
            {
                return;
            }

            this.internalLogger = loggerMethodInfo.Invoke(null, new object[] { Assembly.GetCallingAssembly(), declaringType }); //Assembly.GetCallingAssembly()
        }
       
        /// <summary>
        /// Override the wrapper for log4net ILog IsDebugEnabled
        /// </summary>
        public override bool IsDebugEnabled
        {
            get
            {
                if (!isDebugEnabled.HasValue)
                {
                    if (loadState != LoadState.Success || this.internalLogger == null || loggerType == null || systemStringFormatType == null || debugLevelPropertyValue == null)
                        isDebugEnabled = false;
                    else
                        isDebugEnabled = Convert.ToBoolean(isEnabledForMethod.Invoke(this.internalLogger, new object[] { debugLevelPropertyValue }));
                }
                return isDebugEnabled.Value;
            }
        }

        /// <summary>
        /// Override the wrapper for log4net ILog IsErrorEnabled
        /// </summary>
        public override bool IsErrorEnabled
        {
            get
            {
                if (!isErrorEnabled.HasValue)
                {
                    if (loadState != LoadState.Success || this.internalLogger == null || loggerType == null || systemStringFormatType == null || errorLevelPropertyValue == null)
                        isErrorEnabled = false;
                    else
                        isErrorEnabled = Convert.ToBoolean(isEnabledForMethod.Invoke(this.internalLogger, new object[] { errorLevelPropertyValue }));
                }
                return isErrorEnabled.Value;
            }
        }

        /// <summary>
        /// Simple wrapper around the log4net IsInfoEnabled property.
        /// </summary>
        public override bool IsInfoEnabled
        {
            get
            {
                if (!isInfoEnabled.HasValue)
                {
                    if (loadState != LoadState.Success || this.internalLogger == null || loggerType == null || systemStringFormatType == null || infoLevelPropertyValue == null)
                        isInfoEnabled = false;
                    else
                        isInfoEnabled = Convert.ToBoolean(isEnabledForMethod.Invoke(this.internalLogger, new object[] { infoLevelPropertyValue }));
                }
                return isInfoEnabled.Value;
            }
        }

        /// <summary>
        /// Override the wrapper for log4net ILog Debug
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public override void Debug(System.Exception exception, string messageFormat, params object[] args)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, debugLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, messageFormat, args),
                    exception
                });
        }

        /// <summary>
        /// Override the wrapper for log4net ILog DebugFormat
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public override void DebugFormat(string message, params object[] arguments)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, debugLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, message, arguments),
                    null
                });

        }

        /// <summary>
        /// Override the wrapper for log4net ILog Error
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public override void Error(System.Exception exception, string messageFormat, params object[] args)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, errorLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, messageFormat, args),
                    exception
                });
        }

        /// <summary>
        /// Override the flush
        /// </summary>
        public override void Flush() { }                       

        /// <summary>
        /// Simple wrapper around the log4net InfoFormat method.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public override void InfoFormat(string message, params object[] arguments)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, infoLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, message, arguments),
                    null
                });
        }
    }
}
