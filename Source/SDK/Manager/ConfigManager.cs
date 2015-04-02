using System;
using System.Collections.Generic;
using System.Configuration;
using PayPal.Exception;
using System.Reflection;

namespace PayPal.Manager
{    
    /// <summary>
    /// ConfigManager loads the configuration file and hands out appropriate parameters to application
    /// </summary>
    public sealed class ConfigManager
    {
        /// <summary>
        /// The configValue is readonly as it should not be changed outside constructor (but the content can)
        /// </summary>
        private readonly Dictionary<string, string> configValues;

        private static readonly Dictionary<string, string> defaultConfig;

        /// <summary>
        /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// </summary>
        static ConfigManager() 
        {
            defaultConfig = new Dictionary<string, string>();
            // Default connection timeout in milliseconds
            defaultConfig[BaseConstants.HttpConnectionTimeoutConfig] = "30000";
            defaultConfig[BaseConstants.HttpConnectionRetryConfig] = "3";
            defaultConfig[BaseConstants.ClientIPAddressConfig] = "127.0.0.1";
        }

        /// <summary>
        /// Singleton instance of the ConfigManager
        /// </summary>
        private static volatile ConfigManager singletonInstance;

        private static object syncRoot = new Object();


        /// <summary>
        /// Gets the Singleton instance of the ConfigManager
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                if (singletonInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (singletonInstance == null)
                            singletonInstance = new ConfigManager();
                    }
                }
                return singletonInstance;
            }
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private ConfigManager()
        {
            object paypalConfigSection = null;

            try
            {
                paypalConfigSection = ConfigurationManager.GetSection("paypal");
            }
            catch (System.Exception ex)
            {
                throw new ConfigException("Unable to load 'paypal' section from *.config: " + ex.Message);
            }

            if (paypalConfigSection == null)
            {
                throw new ConfigException("Cannot parse *.config file. Ensure you have configured the 'paypal' section correctly.");
            }
            this.configValues = new Dictionary<string, string>();

            NameValueConfigurationCollection settings = (NameValueConfigurationCollection)paypalConfigSection.GetType().GetProperty("Settings").GetValue(paypalConfigSection, null);
            foreach (string key in settings.AllKeys)
            {
                this.configValues.Add(settings[key].Name, settings[key].Value);
            }

            int index = 0;
            foreach (ConfigurationElement element in (ConfigurationElementCollection)paypalConfigSection.GetType().GetProperty("Accounts").GetValue(paypalConfigSection, null))
            {
                Account account = (Account)element;
                if (!string.IsNullOrEmpty(account.APIUserName))
                {
                    this.configValues.Add("account" + index + ".apiUsername", account.APIUserName);
                }
                if (!string.IsNullOrEmpty(account.APIPassword))
                {
                    this.configValues.Add("account" + index + ".apiPassword", account.APIPassword);
                }
                if (!string.IsNullOrEmpty(account.APISignature))
                {
                    this.configValues.Add("account" + index + ".apiSignature", account.APISignature);
                }
                if (!string.IsNullOrEmpty(account.APICertificate))
                {
                    this.configValues.Add("account" + index + ".apiCertificate", account.APICertificate);
                }
                if (!string.IsNullOrEmpty(account.PrivateKeyPassword))
                {
                    this.configValues.Add("account" + index + ".privateKeyPassword", account.PrivateKeyPassword);
                }
                if (!string.IsNullOrEmpty(account.CertificateSubject))
                {
                    this.configValues.Add("account" + index + ".subject", account.CertificateSubject);
                }
                if (!string.IsNullOrEmpty(account.ApplicationId))
                {
                    this.configValues.Add("account" + index + ".applicationId", account.ApplicationId);
                }
                index++;
            }
        }

        /// <summary>
        /// Returns all properties from the config file
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetProperties()
        {
            // returns a copy of the configuration properties
            return new Dictionary<string, string>(this.configValues);

        }
    
        /// <summary>
        /// Creates new configuration that combines incoming configuration dictionary
        /// and defaults
        /// </summary>
        /// <returns>Default configuration dictionary</returns>
        public static Dictionary<string, string> GetConfigWithDefaults(Dictionary<string, string> config)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>(config);
            foreach (string key in ConfigManager.defaultConfig.Keys)
            {
                if (!ret.ContainsKey(key))
                {
                    ret.Add(key, ConfigManager.defaultConfig[key]);
                }
            }
            return ret;
        }

        public static string GetDefault(string configKey)
        {
            if (ConfigManager.defaultConfig.ContainsKey(configKey))
            {
                return ConfigManager.defaultConfig[configKey];
            }
            return null;
        }
    }
}
