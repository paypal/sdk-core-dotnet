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
        private static IDictionary<Type, Log4netWrapper> cachedLoggers = new Dictionary<Type, Log4netWrapper>();

        private List<BaseLogger> loggers;

        private static Log4netWrapper emptyLogger = new Log4netWrapper();

        private Log4netWrapper()
        {
            loggers = new List<BaseLogger>();
        }

        private Log4netWrapper(Type type)
        {
            loggers = new List<BaseLogger>();

            Log4netReflection log4netLogger = new Log4netReflection(type);
            loggers.Add(log4netLogger);
            DiagnosticsWrapper sdLogger = new DiagnosticsWrapper(type);
            loggers.Add(sdLogger);

            ConfigureLoggers();
            LogConfiguration.PropertyChanged += ConfigsChanged;
        }

        private void ConfigsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && string.Equals(e.PropertyName, "Logging", StringComparison.Ordinal))
            {
                ConfigureLoggers();
            }
        }

        private void ConfigureLoggers()
        {
            LoggerTypes logTypes = LogConfiguration.Logging;

            foreach (BaseLogger il in loggers)
            {
                if (il is Log4netReflection)
                {
                    il.IsEnabled = (logTypes & LoggerTypes.LOG4NET) == LoggerTypes.LOG4NET;
                }

                if (il is DiagnosticsWrapper)
                {
                    il.IsEnabled = (logTypes & LoggerTypes.DIAGNOSTICS) == LoggerTypes.DIAGNOSTICS;
                }
            }
        }

        public static Log4netWrapper GetLogger(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            Log4netWrapper l;
            lock (cachedLoggers)
            {
                if (!cachedLoggers.TryGetValue(type, out l))
                {
                    l = new Log4netWrapper(type);
                    cachedLoggers[type] = l;
                }
            }
            return l;
        }

        public static Log4netWrapper EmptyLogger { get { return emptyLogger; } }


        public void Flush()
        {
            foreach (BaseLogger logger in loggers)
            {
                logger.Flush();
            }
        }

        public void Error(System.Exception exception, string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggers)
            {
                if (logger.IsEnabled && logger.IsErrorEnabled)
                    logger.Error(exception, messageFormat, args);
            }
        }

        public void Debug(System.Exception exception, string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggers)
            {
                if (logger.IsEnabled && logger.IsDebugEnabled)
                    logger.Debug(exception, messageFormat, args);
            }
        }

        public void DebugFormat(string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggers)
            {
                if (logger.IsEnabled && logger.IsDebugEnabled)
                    logger.DebugFormat(messageFormat, args);
            }
        }

        public void InfoFormat(string messageFormat, params object[] args)
        {
            foreach (BaseLogger logger in loggers)
            {
                if (logger.IsEnabled && logger.IsInfoEnabled)
                    logger.InfoFormat(messageFormat, args);
            }
        }

    }
}
