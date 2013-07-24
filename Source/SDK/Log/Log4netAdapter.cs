using System;
using System.Reflection;
using System.Globalization;

namespace PayPal.Log
{
    /// <summary>
    /// Wrapper for reflected log4net logging methods
    /// </summary>
    internal class Log4netAdapter : BaseLogger
    {
        enum Status { NotInitialized, Failure, Loading, Success };

        static Status currentStatus = Status.NotInitialized;
        static readonly object syncLock = new object();

        static Type log4netLoggerType;

        static Type log4netLoggerManger;
        static MethodInfo log4netLoggerMangerMethod;
        private object log4netLoggerMangerMethodInvoke;

        static Type log4netILogger;

        static Type log4netLevel;
        static object log4netLevelDebug;
        static object log4netLevelInfo;
        static object log4netLevelError;

        static MethodInfo log4netILoggerMethodLog;
        static MethodInfo log4netILoggerMethodIsEnabledFor;

        static Type log4netSystemStringFormat;              

        private bool? isErrorEnabled;
        private bool? isDebugEnabled;
        private bool? isInfoEnabled;

        /// <summary>
        /// Interrogate log4net
        /// </summary>
        private static void Reflect()
        {
            lock (Log4netAdapter.syncLock)
            {
                if (currentStatus != Status.NotInitialized)
                {
                    return;
                }

                currentStatus = Status.Loading;
                try
                {
                    log4netLoggerType = Type.GetType("PayPal.Log.Log4netLogger");
                    log4netLoggerManger = Type.GetType("log4net.Core.LoggerManager, log4net");

                    if (log4netLoggerManger == null)
                    {
                        currentStatus = Status.Failure;
                        return;
                    }

                    log4netLoggerMangerMethod = log4netLoggerManger.GetMethod("GetLogger", new Type[] { typeof(Assembly), typeof(Type) });

                    log4netILogger = Type.GetType("log4net.Core.ILogger, log4net");
                    log4netLevel = Type.GetType("log4net.Core.Level, log4net");

                    log4netLevelDebug = log4netLevel.GetField("Debug").GetValue(null);
                    log4netLevelInfo = log4netLevel.GetField("Info").GetValue(null);
                    log4netLevelError = log4netLevel.GetField("Error").GetValue(null);

                    log4netSystemStringFormat = Type.GetType("log4net.Util.SystemStringFormat, log4net");

                    log4netILoggerMethodLog = log4netILogger.GetMethod("Log", new Type[] { typeof(Type), log4netLevel, typeof(object), typeof(System.Exception) });
                    log4netILoggerMethodIsEnabledFor = log4netILogger.GetMethod("IsEnabledFor", new Type[] { log4netLevel });

                    if (log4netLoggerMangerMethod == null || 
                        log4netILoggerMethodIsEnabledFor == null || 
                        log4netILogger == null ||
                        log4netLevel == null ||
                        log4netILoggerMethodLog == null)
                    {
                        currentStatus = Status.Failure;
                        return;
                    }

                    if ((LogConfiguration.Logging & Loggers.Log4net) == Loggers.Log4net)
                    {
                        Type log4netXmlConfigurator = Type.GetType("log4net.Config.XmlConfigurator, log4net");
                        if (log4netXmlConfigurator != null)
                        {
                            MethodInfo log4netXmlConfiguratorMethod = log4netXmlConfigurator.GetMethod("Configure", Type.EmptyTypes);
                            if (log4netXmlConfiguratorMethod != null)
                            {
                                log4netXmlConfiguratorMethod.Invoke(null, null);
                            }
                        }
                    }

                    currentStatus = Status.Success;
                }
                catch
                {
                    currentStatus = Status.Failure;
                }
            }
        }

        public Log4netAdapter(Type givenType) : base(givenType)
        {
            if (currentStatus == Status.NotInitialized)
            {
                Reflect();
            }

            if (log4netLoggerManger == null)
            {
                return;
            }

            this.log4netLoggerMangerMethodInvoke = log4netLoggerMangerMethod.Invoke(null, new object[] { Assembly.GetCallingAssembly(), givenType }); 
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
                    if (currentStatus != Status.Success ||
                        this.log4netLoggerMangerMethodInvoke == null ||
                        log4netLoggerType == null ||
                        log4netSystemStringFormat == null ||
                        log4netLevelDebug == null)
                    {
                        isDebugEnabled = false;
                    }
                    else
                    {
                        isDebugEnabled = Convert.ToBoolean(log4netILoggerMethodIsEnabledFor.Invoke(this.log4netLoggerMangerMethodInvoke, new object[] { log4netLevelDebug }));
                    }
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
                    if (currentStatus != Status.Success ||
                        this.log4netLoggerMangerMethodInvoke == null ||
                        log4netLoggerType == null ||
                        log4netSystemStringFormat == null ||
                        log4netLevelError == null)
                    {
                        isErrorEnabled = false;
                    }
                    else
                    {
                        isErrorEnabled = Convert.ToBoolean(log4netILoggerMethodIsEnabledFor.Invoke(this.log4netLoggerMangerMethodInvoke, new object[] { log4netLevelError }));
                    }
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
                    if (currentStatus != Status.Success ||
                        this.log4netLoggerMangerMethodInvoke == null ||
                        log4netLoggerType == null ||
                        log4netSystemStringFormat == null ||
                        log4netLevelInfo == null)
                    {
                        isInfoEnabled = false;
                    }
                    else
                    {
                        isInfoEnabled = Convert.ToBoolean(log4netILoggerMethodIsEnabledFor.Invoke(this.log4netLoggerMangerMethodInvoke, new object[] { log4netLevelInfo }));
                    }
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
            log4netILoggerMethodLog.Invoke(
                this.log4netLoggerMangerMethodInvoke,
                new object[]
                {
                    log4netLoggerType, log4netLevelDebug,
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
            log4netILoggerMethodLog.Invoke(
                this.log4netLoggerMangerMethodInvoke,
                new object[]
                {
                    log4netLoggerType, 
                    log4netLevelDebug,
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
            log4netILoggerMethodLog.Invoke(
                this.log4netLoggerMangerMethodInvoke,
                new object[]
                {
                    log4netLoggerType, 
                    log4netLevelError,
                    new LogMessage(CultureInfo.InvariantCulture, messageFormat, args),
                    exception
                });
        }

        /// <summary>
        /// Override the flush
        /// </summary>
        public override void Flush() { }                       

        /// <summary>
        /// Override the wrapper for log4net ILog InfoFormat
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public override void InfoFormat(string message, params object[] arguments)
        {
            log4netILoggerMethodLog.Invoke
            (
                this.log4netLoggerMangerMethodInvoke,
                new object[]
                {
                    log4netLoggerType, 
                    log4netLevelInfo,
                    new LogMessage(CultureInfo.InvariantCulture, message, arguments),
                    null
                }
            );
        }
    }
}
