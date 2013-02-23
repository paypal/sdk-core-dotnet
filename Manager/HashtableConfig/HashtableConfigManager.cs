using System.Collections.Generic;

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
        public HashtableConfigManager(Dictionary<string, object> config)
        {
            var settings = new Dictionary<string, string>();
            var accounts = new SortedList<int, Dictionary<string, string>>();
            var accountIndex = 0;

            foreach (KeyValuePair<string, object> elementEntry in config)
            {
                if (elementEntry.Key == "accounts")
                {
                    foreach (KeyValuePair<int, Dictionary<string, string>> accountEntry in (SortedList<int, Dictionary<string, string>>)elementEntry.Value)
                    {
                        var account = new Dictionary<string, string>();
                        foreach (var accountEntryValue in accountEntry.Value)
                        {
                            account.Add(accountEntryValue.Key, accountEntryValue.Value);
                        }
                        accounts.Add(accountIndex++, account);
                    }
                }
                else
                {
                    settings.Add(elementEntry.Key, elementEntry.Value.ToString());
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
            foreach (var account in _config.Accounts.Values)
            {
                if (account.APIUsername == apiUserName)
                {
                    return account;
                }
                
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
            return _config.Accounts.Values[index];
        }        
    }
}
