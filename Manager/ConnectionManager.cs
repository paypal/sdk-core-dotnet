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

        private readonly IConfigManager _configMgr;

        /// <summary>
        /// Private constructor
        /// </summary>
        public ConnectionManager(IConfigManager configMgr)
        {
            _configMgr = configMgr;
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

            //Set request proxy for tunnelling http requests via a proxy server
            string proxyAddress = _configMgr.GetProperty(BaseConstants.HTTP_PROXY_ADDRESS);
            if (proxyAddress != null)
            {
                WebProxy requestProxy = new WebProxy();
                requestProxy.Address = new Uri(proxyAddress);
                string proxyCredentials = _configMgr.GetProperty(BaseConstants.HTTP_PROXY_CREDENTIAL);
                if (proxyCredentials != null)
                {
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
