using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;

namespace PayPal.Log
{
    /// <summary>
    /// Configuration for PayPalCoreSDK
    /// </summary>
    public static class LogConfiguration
    {
        /// <summary>
        /// Key for the loggers to be set in <appSettings><add key="PayPalLogger" value="PayPal.Log.DiagnosticsLogger, PayPal.Log.Log4netLogger"/></appSettings> in configuration file
        /// </summary>
        public const string PayPalLogKey = "PayPalLogger";

        private static char[] splitters = new char[] { ',' };

        private static List<string> configurationLoggerList = GetConfigurationLoggerList();
               
        public static List<string> LoggerListInConfiguration
        {
            get
            {
                return configurationLoggerList;
            }
        }

        private static List<string> GetConfigurationLoggerList()
        {
            List<string> loggerList = new List<string>();

            string value = GetConfiguration(PayPalLogKey);

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            List<string> splitList = new List<string>(value.Split(splitters, StringSplitOptions.RemoveEmptyEntries));

            if (splitList == null || splitList.Count == 0)
            {
                return null;
            }                        

            foreach(string split in splitList)
            {
                if (!loggerList.Contains(split.Trim()))
                {
                    loggerList.Add(split.Trim());
                }
            }

            return loggerList;
        }     

        private static string GetConfiguration(string name)
        {
            NameValueCollection appSetting = ConfigurationManager.AppSettings;

            if (appSetting == null)
            {
                return null;
            }

            string value = appSetting[name];
            return value;
        }

        private static bool GetConfigBool(string name)
        {
            string value = GetConfiguration(name);
            bool result;

            if (bool.TryParse(value, out result))
            {
                return result;
            }

            return default(bool);
        }
    }   
}
