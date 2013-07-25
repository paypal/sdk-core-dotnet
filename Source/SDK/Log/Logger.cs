using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using PayPal.Exception;

namespace PayPal.Log
{
    internal class Logger
    {
        private static IDictionary<Type, Logger> loggerKeyValuePairs = new Dictionary<Type, Logger>();

        private List<BaseLogger> baseLoggerList;

        private Logger(Type givenType)
        {
            baseLoggerList = new List<BaseLogger>();

            if (LogConfiguration.LoggerListInConfiguration != null)
            {
                foreach (string loggerName in LogConfiguration.LoggerListInConfiguration)
                {
                    Type loggerType = Type.GetType(loggerName);

                    if (loggerType != null)
                    {
                        Type[] types = { typeof(Type) };
                        ConstructorInfo infoConstructor = loggerType.GetConstructor( types );

                        if (infoConstructor != null)
                        {
                            try
                            {
                                object instance = infoConstructor.Invoke(new object[] { givenType });

                                if (instance is BaseLogger)
                                {
                                    baseLoggerList.Add((BaseLogger)instance);
                                }
                            }
                            catch
                            {
                                throw new ConfigException("Invalid Configuration. Please check 'PayPalLog' value in  <appSettings> section of your configuration file.");
                            }                            
                        }
                    }
                }

                if (baseLoggerList.Count > 0)
                {
                    ConfigureLoggers();
                }
            }
        }       

        private void ConfigureLoggers()
        { 
            foreach (BaseLogger loggerBase in baseLoggerList)
            {
                loggerBase.IsEnabled = true;              
            }            
        }

        public static Logger GetLogger(Type givenType)
        {
            if (givenType == null)
            {
                throw new ArgumentNullException("Type cannot be null");
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
