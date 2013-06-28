using System;
using System.Collections.Generic;
using System.Net;
/* NuGet Install
 * Visual Studio 2008
    * Install log4net -OutputDirectory .\packages
    * Add reference from the folder "net35-full"
 * Visual Studio 2010 or higher
    * Install-Package log4net
    * Reference is auto-added 
*/
using log4net;
using PayPal.Exception;

namespace PayPal.Manager
{
    /// <summary>
    ///  ConnectionManager retrieves HttpConnection objects used by API service
    /// </summary>
    public sealed class ConnectionManager
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static ILog Logger = LogManagerWrapper.GetLogger(typeof(ConnectionManager));

#if NET_2_0
        /// <summary>
        /// Singleton instance of ConnectionManager
        /// </summary>
        private static readonly ConnectionManager SingletonInstance = new ConnectionManager();

        /// <summary>
        /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// </summary>
        static ConnectionManager() { }

        /// <summary>
        /// Gets the Singleton instance of ConnectionManager
        /// </summary>
        public static ConnectionManager Instance
        {
            get
            {
                return SingletonInstance;
            }
        }
#elif NET_3_5
        /// <summary>
        /// Singleton instance of ConnectionManager
        /// </summary>
        private static readonly ConnectionManager SingletonInstance = new ConnectionManager();

        /// <summary>
        /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// </summary>
        static ConnectionManager() { }

        /// <summary>
        /// Gets the Singleton instance of ConnectionManager
        /// </summary>
        public static ConnectionManager Instance
        {
            get
            {
                return SingletonInstance;
            }
        }
#elif NET_4_0
        /// <summary>
        /// System.Lazy type guarantees thread-safe lazy-construction
        /// static holder for instance, need to use lambda to construct since constructor private
        /// </summary>
        private static readonly Lazy<ConnectionManager> laze = new Lazy<ConnectionManager>(() => new ConnectionManager());

        /// <summary>
        /// Accessor for the Singleton instance of ConnectionManager
        /// </summary>
        public static ConnectionManager Instance { get { return laze.Value; } }  
#endif
        /// <summary>
        /// Private constructor, private to prevent direct instantiation
        /// </summary>
        private ConnectionManager() { }     

        /// <summary>
        /// Create and Config a HttpWebRequest
        /// </summary>
        /// <param name="config">Config properties</param>
        /// <param name="url">Url to connect to</param>
        /// <returns></returns>
        public HttpWebRequest GetConnection(Dictionary<string, string> config, string url)
        {

            HttpWebRequest httpRequest = null;                        
            try
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            catch (UriFormatException ex)
            {
                Logger.Error(ex.Message);
                throw new ConfigException("Invalid URI " + url);
            }

            // Set connection timeout
            int ConnectionTimeout = 0;
            if(!config.ContainsKey(BaseConstants.HttpConnectionTimeoutConfig) ||
                !int.TryParse(config[BaseConstants.HttpConnectionTimeoutConfig], out ConnectionTimeout)) {
                int.TryParse(ConfigManager.getDefault(BaseConstants.HttpConnectionTimeoutConfig), out ConnectionTimeout);
            }            
            httpRequest.Timeout = ConnectionTimeout;

            // Set request proxy for tunnelling http requests via a proxy server
            if(config.ContainsKey(BaseConstants.HttpProxyAddressConfig))
            {
                WebProxy requestProxy = new WebProxy();
                requestProxy.Address = new Uri(config[BaseConstants.HttpProxyAddressConfig]);                
                if (config.ContainsKey(BaseConstants.HttpProxyCredentialConfig))
                {
                    string proxyCredentials = config[BaseConstants.HttpProxyCredentialConfig];
                    string[] proxyDetails = proxyCredentials.Split(':');
                    if (proxyDetails.Length == 2)
                    {
                        requestProxy.Credentials = new NetworkCredential(proxyDetails[0], proxyDetails[1]);
                    }
                }                
                httpRequest.Proxy = requestProxy;
            }
            return httpRequest;
        }
    }
}
