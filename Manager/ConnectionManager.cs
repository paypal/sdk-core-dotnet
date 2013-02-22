using System;
using System.Net;
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
        private static ILog logger = LogManagerWrapper.GetLogger(typeof(ConnectionManager));
        
        /// <summary>
        /// Singleton instance of ConnectionManager
        /// </summary>
        private static readonly ConnectionManager singletonInstance = new ConnectionManager();

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static ConnectionManager() { }
        
        /// <summary>
        /// Private constructor
        /// </summary>
        private ConnectionManager() { }

        /// <summary>
        /// Gets the Singleton instance of ConnectionManager
        /// </summary>
        public static ConnectionManager Instance
        {
            get
            {
                return singletonInstance;
            }
        }

        /// <summary>
        /// Create and Config a HttpWebRequest
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public HttpWebRequest GetConnection(string url)
        {
            HttpWebRequest httpRequest = null;
                        
            try
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            catch (UriFormatException ex)
            {
                logger.Debug(ex.Message);
                throw new ConfigException("Invalid URI " + url);
            }

            // Set connection timeout
            httpRequest.Timeout = BaseConstants.DEFAULT_TIMEOUT;

            // Set request proxy for tunnelling http requests via a proxy server
            //string proxyAddress = configMngr.GetProperty(BaseConstants.HTTP_PROXY_ADDRESS);
            //if (proxyAddress != null)
            //{
            //    WebProxy requestProxy = new WebProxy();
            //    requestProxy.Address = new Uri(proxyAddress);
            //    string proxyCredentials = configMngr.GetProperty(BaseConstants.HTTP_PROXY_CREDENTIAL);
            //    if (proxyCredentials != null)
            //    {
            //        string[] proxyDetails = proxyCredentials.Split(':');
            //        if (proxyDetails.Length == 2)
            //        {
            //            requestProxy.Credentials = new NetworkCredential(proxyDetails[0], proxyDetails[1]);
            //        }
            //    }                
            //    httpRequest.Proxy = requestProxy;
            //}

            return httpRequest;
        }
    }
}
