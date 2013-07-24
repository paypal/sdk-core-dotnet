using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PayPal.Log
{
    /// <summary>
    /// Log4net wrapper to do without log4net in the SDK distribution
    /// </summary>
    internal class Log4netLogger
    {
        private static IDictionary<Type, Log4netLogger> loggerKeyValuePairs = new Dictionary<Type, Log4netLogger>();

        private List<BaseLogger> loggerList;

        private static Log4netLogger loggerLog4net = new Log4netLogger();

        private Log4netLogger()
        {
            loggerList = new List<BaseLogger>();
        }

        private Log4netLogger(Type givenType)
        {
            loggerList = new List<BaseLogger>();

            Log4netAdapter adapterLog4net = new Log4netAdapter(givenType);
            loggerList.Add(adapterLog4net);

            DiagnosticsLogger loggerDiagnostics = new DiagnosticsLogger(givenType);
            loggerList.Add(loggerDiagnostics);

            ConfigureLoggers();            
        }       

        private void ConfigureLoggers()
        {
            //List<string> configuredLoggers = LogConfiguration.LoggerList;
            //foreach (string logger in configuredLoggers)
            //{

            //}

            Loggers loggerTypes = LogConfiguration.Logging;

            foreach (BaseLogger logger in loggerList)
            {
                if (logger is Log4netAdapter)
                {
                    logger.IsEnabled = (loggerTypes & Loggers.Log4net) == Loggers.Log4net;
                }

                if (logger is DiagnosticsLogger)
                {
                    logger.IsEnabled = (loggerTypes & Loggers.Diagnostics) == Loggers.Diagnostics;
                }
            }            
        }

        public static Log4netLogger GetLog4netLogger(Type givenType)
        {
            if (givenType == null)
            {
                throw new ArgumentNullException("type");
            }

            Log4netLogger logger;
            lock (loggerKeyValuePairs)
            {
                if (!loggerKeyValuePairs.TryGetValue(givenType, out logger))
                {
                    logger = new Log4netLogger(givenType);
                    loggerKeyValuePairs[givenType] = logger;
                }
            }
            return logger;
        }

        public void Flush()
        {
            foreach (BaseLogger logger in loggerList)
            {
                logger.Flush();
            }
        }

        public void Error(System.Exception exception, string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggerList)
            {
                if (logger.IsEnabled && logger.IsErrorEnabled)
                {
                    logger.Error(exception, messageFormat, args);
                }
            }
        }

        public void Debug(System.Exception exception, string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggerList)
            {
                if (logger.IsEnabled && logger.IsDebugEnabled)
                {
                    logger.Debug(exception, messageFormat, args);
                }
            }
        }

        public void DebugFormat(string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggerList)
            {
                if (logger.IsEnabled && logger.IsDebugEnabled)
                {
                    logger.DebugFormat(messageFormat, args);
                }
            }
        }

        public void InfoFormat(string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggerList)
            {
                if (logger.IsEnabled && logger.IsInfoEnabled)
                {
                    logger.InfoFormat(messageFormat, args);
                }
            }
        }

    }
}
