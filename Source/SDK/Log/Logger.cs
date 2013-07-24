using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PayPal.Log
{
    internal class Logger
    {
        private static IDictionary<Type, Logger> loggerKeyValuePairs = new Dictionary<Type, Logger>();

        private List<BaseLogger> baseLoggerList;

        private Logger(Type givenType)
        {
            baseLoggerList = new List<BaseLogger>();

            Log4netLogger loggerLog4net = new Log4netLogger(givenType);
            baseLoggerList.Add(loggerLog4net);

            DiagnosticsLogger loggerDiagnostics = new DiagnosticsLogger(givenType);
            baseLoggerList.Add(loggerDiagnostics);

            ConfigureLoggers();            
        }       

        private void ConfigureLoggers()
        {
            LoggerTypes typesLogger = LogConfiguration.LoggersInConfiguration;

            foreach (BaseLogger loggerBase in baseLoggerList)
            {
                if (loggerBase is Log4netLogger)
                {
                    loggerBase.IsEnabled = (typesLogger & LoggerTypes.Log4net) == LoggerTypes.Log4net;
                }

                if (loggerBase is DiagnosticsLogger)
                {
                    loggerBase.IsEnabled = (typesLogger & LoggerTypes.Diagnostics) == LoggerTypes.Diagnostics;
                }
            }            
        }

        public static Logger GetLogger(Type givenType)
        {
            if (givenType == null)
            {
                throw new ArgumentNullException("type");
            }

            Logger log;
            lock (loggerKeyValuePairs)
            {
                if (!loggerKeyValuePairs.TryGetValue(givenType, out log))
                {
                    log = new Logger(givenType);
                    loggerKeyValuePairs[givenType] = log;
                }
            }
            return log;
        }

        public void Flush()
        {
            foreach (BaseLogger loggerBase in baseLoggerList)
            {
                loggerBase.Flush();
            }
        }              

        public void Debug(System.Exception exception, string messageFormat, params object[] args)
        {
            foreach (BaseLogger loggerBase in baseLoggerList)
            {
                if (loggerBase.IsEnabled && loggerBase.IsDebugEnabled)
                {
                    loggerBase.Debug(exception, messageFormat, args);
                }
            }
        }

        public void DebugFormat(string message, params object[] args)
        {
            foreach (BaseLogger loggerBase in baseLoggerList)
            {
                if (loggerBase.IsEnabled && loggerBase.IsDebugEnabled)
                {
                    loggerBase.DebugFormat(message, args);
                }
            }
        }

        public void Error(System.Exception exception, string messageFormat, params object[] args)
        {
            foreach (BaseLogger loggerBase in baseLoggerList)
            {
                if (loggerBase.IsEnabled && loggerBase.IsErrorEnabled)
                {
                    loggerBase.Error(exception, messageFormat, args);
                }
            }
        }       

        public void InfoFormat(string message, params object[] args)
        {
            foreach (BaseLogger loggerBase in baseLoggerList)
            {
                if (loggerBase.IsEnabled && loggerBase.IsInfoEnabled)
                {
                    loggerBase.InfoFormat(message, args);
                }
            }
        }

    }
}
