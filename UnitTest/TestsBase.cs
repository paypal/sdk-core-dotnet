using System.Collections;
using System.Collections.Generic;
using PayPal.Manager;
using PayPal.Manager.AppConfig;
using PayPal.Manager.HashtableConfig;

namespace PayPal.UnitTest
{
    public class TestsBase
    {
        protected readonly AppConfigManager AppConfigMgr;
        protected readonly CredentialManager AppCredentialMgr;
        protected readonly ConnectionManager AppConnMgr;

        protected readonly IConfigManager HashtableConfigMgr;
        protected readonly CredentialManager HashtableCredentialMgr;
        protected readonly ConnectionManager HashtableConnMgr;

        public TestsBase()
        {
            var configSettings =
                new Hashtable
                    {
                        { "endpoint", "https://svcs.sandbox.paypal.com/" },
                        { "PayPalAPI", "https://svcs.sandbox.paypal.com/" },
                        { "PayPalAPIAA", "https://svcs.sandbox.paypal.com/" },
                        { "AdaptiveAccounts", "https://svcs.sandbox.paypal.com/" },
                        { "AdaptivePayments", "https://svcs.sandbox.paypal.com/" },
                        { "Invoice", "https://svcs.sandbox.paypal.com/" },
                        { "Permissions", "https://svcs.sandbox.paypal.com/" },
                        { "IPNEndpoint", "https://www.sandbox.paypal.com/cgi-bin/webscr" },
                        { "connectionTimeout", "3600000" },
                        { "requestRetries", "3" },
                        { "IPAddress", "127.0.0.1" },
                        { "sandboxEmailAddress", "Platform.sdk.seller@gmail.com" },
                        {
                            "accounts", 
                            new Dictionary<string, Dictionary<string, string>>
                            {
                                { 
                                    "jb-us-seller_api1.paypal.com", 
                                    new Dictionary<string, string>
                                    { 
                                        { "apiUsername", "jb-us-seller_api1.paypal.com" },
                                        { "apiPassword", "WX4WTU3S8MY44S7F" },
                                        { "apiSignature", "AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy" },
                                        { "applicationId", "APP-80W284485P519543T" }
                                    }
                                },
                                {
                                    "certuser_biz_api1.paypal.com", 
                                    new Dictionary<string, string>
                                    { 
                                        { "apiUsername", "certuser_biz_api1.paypal.com" },
                                        { "apiPassword", "D6JNKKULHN3G5B8A" },
                                        { "applicationId", "APP-80W284485P519543T" },
                                        { "apiCertificate", @"C:\git\sdk-core-dotnet\UnitTest\Resources\sdk-cert.p12" },
                                        { "privateKeyPassword", "password" }
                                    }
                                }
                            }
                        }
                    };

            HashtableConfigMgr = new HashtableConfigManager(configSettings);
            HashtableCredentialMgr = new CredentialManager(HashtableConfigMgr);
            HashtableConnMgr = new ConnectionManager(HashtableConfigMgr);

            AppConfigMgr = new AppConfigManager();
            AppCredentialMgr = new CredentialManager(AppConfigMgr);
            AppConnMgr = new ConnectionManager(AppConfigMgr);
        }
    }
}
