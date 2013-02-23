using System.Collections.Generic;
using System.Configuration;

namespace PayPal.Manager
{
    public interface ISDKConfigHandler : IConfigurationSectionHandler
    {
        Dictionary<string, IAccount> Accounts { get; }
        string Setting(string name);
    }
}
