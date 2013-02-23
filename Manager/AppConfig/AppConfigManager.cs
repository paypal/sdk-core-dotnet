using System.Configuration;
using PayPal.Exception;

namespace PayPal.Manager.AppConfig
{
    /// <summary>
    /// ConfigManager loads the configuration file and hands out
    /// appropriate parameters to application
    /// </summary>
    public sealed class AppConfigManager : IConfigManager
    {
        private readonly SDKAppConfigHandler _config;

        /// <summary>
        /// Private constructor
        /// </summary>
        public AppConfigManager()
        {
            _config = (SDKAppConfigHandler)ConfigurationManager.GetSection("paypal");
            if (_config == null)
            {
                throw new ConfigException("Cannot read config file");
            }
        }

        /// <summary>
        /// Returns the key from the configuration file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetProperty(string key)
        {
            return _config.Setting(key);
        }

        /// <summary>
        /// Returns the API Username from the configuration file
        /// </summary>
        /// <param name="apiUserName"></param>
        /// <returns></returns>
        public IAccount GetAccount(string apiUserName)
        {
            return _config.Accounts[apiUserName];
        }

        /// <summary>
        /// Returns the index from the configuration file
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IAccount GetAccount(int index)
        {
            return _config.Accounts[index];
        }
    }
}