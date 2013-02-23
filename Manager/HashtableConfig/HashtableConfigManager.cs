using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PayPal.Manager.HashtableConfig
{    
    /// <summary>
    /// ConfigManager loads the configuration file and hands out
    /// appropriate parameters to application
    /// </summary>
    public sealed class HashtableConfigManager : IConfigManager
    {
        private readonly SDKHashtableConfigHandler _config;

        /// <summary>
        /// Private constructor
        /// </summary>
        public HashtableConfigManager(Hashtable config)
        {
            var settings = new Dictionary<string, string>();
            var accounts = new Dictionary<string, Dictionary<string, string>>();

            foreach (DictionaryEntry elementEntry in config)
            {
                if (elementEntry.Key.ToString() == "accounts")
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> accountEntry in (Dictionary<string, Dictionary<string, string>>)elementEntry.Value)
                    {
                        var account = accountEntry.Value.ToDictionary(accountElement => accountElement.Key, accountElement => accountElement.Value);
                        var apiUserName = string.Empty;
                        account.TryGetValue(account.Keys.FirstOrDefault(k => k == "apiUsername"), out apiUserName);
                        accounts.Add(apiUserName, account);
                    }
                }
                else
                {
                    settings.Add(elementEntry.Key.ToString(), elementEntry.Value.ToString());
                }
            }

            _config = new SDKHashtableConfigHandler(settings, accounts);
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
            IAccount account;
            if (_config.Accounts.TryGetValue(apiUserName, out account))
            {
                return account;
            }
            return null;
        }

        /// <summary>
        /// Returns the index from the configuration file
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IAccount GetAccount(int index)
        {
            return _config.Accounts[_config.Accounts.Keys.ElementAt(index)];
        }        
    }
}
