using System;
using System.Collections.Generic;
using System.Configuration;
using PayPal.Exception;

namespace PayPal.Manager
{    
    /// <summary>
    /// ConfigManager loads the configuration file and hands out appropriate parameters to application
    /// </summary>
    public sealed class ConfigManager
    {
        private SDKConfigHandler ConfigHandler;

        private Dictionary<string, string> ConfigValues;

        private static readonly Dictionary<string, string> DefaultConfig;
                
        static ConfigManager()
        {
            DefaultConfig = new Dictionary<string, string>();
            // Default connection timeout in milliseconds
            DefaultConfig[BaseConstants.HttpConnectionTimeoutConfig] = "30000";
            DefaultConfig[BaseConstants.HttpConnectionRetryConfig] = "1";
            DefaultConfig[BaseConstants.ClientIPAddressConfig] = "127.0.0.1";
        }

        /// <summary>
        /// Singleton instance of the ConfigManager
        /// </summary>
        private static volatile ConfigManager SingletonInstance;

        private static object SyncRoot = new Object();

        /// <summary>
        /// Gets the Singleton instance of the ConfigManager
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                if (SingletonInstance == null)
                {
                    lock (SyncRoot)
                    {
                        if (SingletonInstance == null)
                        {
                            SingletonInstance = new ConfigManager();
                        }
                    }
                }
                return SingletonInstance;
            }
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private ConfigManager()
        {
            ConfigHandler = (SDKConfigHandler)ConfigurationManager.GetSection("paypal");
            if (ConfigHandler == null)
            {
                throw new ConfigException("Cannot parse *.Config file. Ensure you have configured the 'paypal' section correctly.");
            }
            this.ConfigValues = new Dictionary<string, string>();

            NameValueConfigurationCollection settings = this.ConfigHandler.Settings;
            foreach (string key in settings.AllKeys)
            {
                this.ConfigValues.Add(settings[key].Name, settings[key].Value);
            }

            int index = 0;
            foreach (ConfigurationElement element in this.ConfigHandler.Accounts)
            {
                Account account = (Account)element;
                if (!string.IsNullOrEmpty(account.APIUserName))
                {
                    this.ConfigValues.Add("account" + index + ".apiUsername", account.APIUserName);
                }
                if (!string.IsNullOrEmpty(account.APIPassword))
                {
                    this.ConfigValues.Add("account" + index + ".apiPassword", account.APIPassword);
                }
                if (!string.IsNullOrEmpty(account.APISignature))
                {
                    this.ConfigValues.Add("account" + index + ".apiSignature", account.APISignature);
                }
                if (!string.IsNullOrEmpty(account.APICertificate))
                {
                    this.ConfigValues.Add("account" + index + ".apiCertificate", account.APICertificate);
                }
                if (!string.IsNullOrEmpty(account.PrivateKeyPassword))
                {
                    this.ConfigValues.Add("account" + index + ".privateKeyPassword", account.PrivateKeyPassword);
                }
                if (!string.IsNullOrEmpty(account.CertificateSubject))
                {
                    this.ConfigValues.Add("account" + index + ".subject", account.CertificateSubject);
                }
                if (!string.IsNullOrEmpty(account.ApplicationID))
                {
                    this.ConfigValues.Add("account" + index + ".applicationId", account.ApplicationID);
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
            return this.ConfigValues;
        }
    
        /// <summary>
        /// Creates new configuration that combines incoming configuration dictionary
        /// and defaults
        /// </summary>
        /// <returns>Default configuration dictionary</returns>
        public static Dictionary<string, string> GetConfigWithDefaults(Dictionary<string, string> config)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>(config);
            foreach (string key in ConfigManager.DefaultConfig.Keys)
            {
                if (!ret.ContainsKey(key))
                {
                    ret.Add(key, DefaultConfig[key]);
                }
            }
            return ret;
        }

        public static string getDefault(string configKey)
        {
            if (ConfigManager.DefaultConfig.ContainsKey(configKey))
            {
                return ConfigManager.DefaultConfig[configKey];
            }
            return null;
        }
    }
}
