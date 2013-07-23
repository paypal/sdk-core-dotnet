using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;

namespace PayPal
{
    /// <summary>
    /// Configuration options that apply to the entire SDK.
    /// </summary>
    public static class PayPalConfiguration
    {
        internal static event PropertyChangedEventHandler PropertyChanged;

        #region Logging

        /// <summary>
        /// Key for the Logging property.
        /// <seealso cref="PayPal.PayPalConfiguration.Logging"/>
        /// </summary>
        public const string LoggingKey = "PayPalLogKey";

        /// <summary>
        /// Configures how the SDK should log events, if at all.
        /// Changes to this setting will only take effect in newly-constructed clients.
        /// 
        /// The setting can be configured through App.config, for example:
        /// <code>
        /// &lt;appSettings&gt;
        ///   &lt;add key="PayPalLogKey" value="log4net"/&gt;
        /// &lt;/appSettings&gt;
        /// </code>
        /// </summary>
        public static LoggerTypes Logging
        {
            get { return _logging; }
            set
            {
                _logging = value;
                OnPropertyChanged("Logging");
            }
        }

        private static char[] validSeparators = new char[] { ' ', ',' };
        private static LoggerTypes _logging = GetLoggingSetting();
        private static LoggerTypes GetLoggingSetting()
        {
            string value = GetConfig(LoggingKey);
            if (string.IsNullOrEmpty(value))
                return LoggerTypes.NONE;

            string[] settings = value.Split(validSeparators, StringSplitOptions.RemoveEmptyEntries);
            if (settings == null || settings.Length == 0)
                return LoggerTypes.NONE;

            LoggerTypes totalSetting = LoggerTypes.NONE;
            foreach (string setting in settings)
            {
                LoggerTypes l = ParseEnum<LoggerTypes>(setting);
                totalSetting |= l;
            }
            return totalSetting;
        }

        #endregion





        #region Private general methods

        private static void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs(name));
            }
        }
        private static string GetConfig(string name)
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;
            if (appConfig == null)
                return null;
            string value = appConfig[name];
            return value;
        }
        private static bool GetConfigBool(string name)
        {
            string value = GetConfig(name);
            bool result;
            if (bool.TryParse(value, out result))
                return result;
            return default(bool);
        }
        private static T GetConfigEnum<T>(string name)
        {
            Type type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException(string.Format("Type {0} must be enum", type.FullName));

            string value = GetConfig(name);
            if (string.IsNullOrEmpty(value))
                return default(T);
            T result = ParseEnum<T>(value);
            return result;
        }
        private static T ParseEnum<T>(string value)
        {
            T t;
            if (TryParseEnum<T>(value, out t))
                return t;
            Type type = typeof(T);
            string messageFormat = "Unable to parse value {0} as enum of type {1}. Valid values are: {2}";
            string enumNames = string.Join(", ", Enum.GetNames(type));
            throw new InvalidEnumArgumentException(string.Format(messageFormat, value, type.FullName, enumNames));
        }

        private static bool TryParseEnum<T>(string value, out T result)
        {
            result = default(T);

            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                T t = (T)Enum.Parse(typeof(T), value, true);
                result = t;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Comma separate the log types to enable multiple loggers
    /// </summary>
    [Flags]
    public enum LoggerTypes
    {
        /// <summary>
        /// No logging
        /// </summary>
        NONE = 0,
        
        /// <summary>
        /// Log using log4net
        /// </summary>
        LOG4NET = 1,
        
        /// <summary>
        /// Log using System.Diagnostics
        /// </summary>
        DIAGNOSTICS = 2
    }   
}
