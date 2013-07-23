using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PayPal.Log
{
    /// <summary>
    /// Log4net wrapper to do without log4net in the SDK distribution
    /// </summary>
    internal class Log4netWrapper
    {
        private static IDictionary<Type, Log4netWrapper> loggerKeyValuePairs = new Dictionary<Type, Log4netWrapper>();

        private List<BaseLogger> loggerList;

        private static Log4netWrapper wrapperLog4net = new Log4netWrapper();

        private Log4netWrapper()
        {
            loggerList = new List<BaseLogger>();
        }

        private Log4netWrapper(Type type)
        {
            loggerList = new List<BaseLogger>();

            Log4netReflection log4netLogger = new Log4netReflection(type);
            loggerList.Add(log4netLogger);
            DiagnosticsWrapper sdLogger = new DiagnosticsWrapper(type);
            loggerList.Add(sdLogger);

            ConfigureLoggers();
            LogConfiguration.PropertyChanged += ConfigurationChanged;
        }

        private void ConfigurationChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && string.Equals(e.PropertyName, "Logging", StringComparison.Ordinal))
            {
                ConfigureLoggers();
            }
        }

        private void ConfigureLoggers()
        {
            Loggers loggerTypes = LogConfiguration.Logging;

            foreach (BaseLogger logger in loggerList)
            {
                if (logger is Log4netReflection)
                {
                    logger.IsEnabled = (loggerTypes & Loggers.Log4net) == Loggers.Log4net;
                }

                if (logger is DiagnosticsWrapper)
                {
                    logger.IsEnabled = (loggerTypes & Loggers.Diagnostics) == Loggers.Diagnostics;
                }
            }
        }

        public static Log4netWrapper GetLogger(Type givenType)
        {
            if (givenType == null) throw new ArgumentNullException("type");

            Log4netWrapper wrapper;
            lock (loggerKeyValuePairs)
            {
                if (!loggerKeyValuePairs.TryGetValue(givenType, out wrapper))
                {
                    wrapper = new Log4netWrapper(givenType);
                    loggerKeyValuePairs[givenType] = wrapper;
                }
            }
            return wrapper;
        }

        public static Log4netWrapper GetLog4netWrapper 
        { 
            get 
            { 
                return wrapperLog4net; 
            } 
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
