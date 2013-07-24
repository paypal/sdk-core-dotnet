using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;

namespace PayPal.Log
{
    /// <summary>
    /// Configuration for PayPalCoreSDK
    /// </summary>
    public static class LogConfiguration
    {
        /// <summary>
        /// Key for the loggers to be set in <appSettings><add key="PayPalLog" value="Log4net"/></appSettings> in configuration file
        /// </summary>
        public const string PayPalLogKey = "PayPalLog";

        private static char[] splitters = new char[] { ',' };

        private static Loggers loggerTypes = GetLoggers();
                
        /// <summary>
        /// Gets and sets the loggers
        /// </summary>
        public static Loggers Logging
        {
            get 
            { 
                return loggerTypes; 
            }          
        }        

        private static Loggers GetLoggers()
        {
            string value = GetConfiguration(PayPalLogKey);
            if (string.IsNullOrEmpty(value))
            {
                return Loggers.None;
            }

            string[] settings = value.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            if (settings == null || settings.Length == 0)
            {
                return Loggers.None;
            }

            Loggers loggerType = Loggers.None;

            foreach (string setting in settings)
            {
                Loggers loggerSet = ParseEnum<Loggers>(setting);
                loggerType |= loggerSet;
            }

            return loggerType;
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

        private static T GetConfigEnum<T>(string name)
        {
            Type typeOfT = typeof(T);
            if (!typeOfT.IsEnum) throw new InvalidOperationException(string.Format("Type {0} must be enum ", typeOfT.FullName));

            string value = GetConfiguration(name);
            if (string.IsNullOrEmpty(value))
                return default(T);
            T result = ParseEnum<T>(value);
            return result;
        }

        private static T ParseEnum<T>(string value)
        {
            T tObject;

            if (TryParseEnum<T>(value, out tObject))
            {
                return tObject;
            }

            Type typeOfT = typeof(T);
            string messageFormat = "Unable to parse value {0} as enum of type {1}. Valid values are: {2}";
            string enumNames = string.Join(", ", Enum.GetNames(typeOfT));

            throw new InvalidEnumArgumentException(string.Format(messageFormat, value, typeOfT.FullName, enumNames));
        }

        private static bool TryParseEnum<T>(string value, out T result)
        {
            result = default(T);

            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                T tObject = (T)Enum.Parse(typeof(T), value, true);
                result = tObject;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
