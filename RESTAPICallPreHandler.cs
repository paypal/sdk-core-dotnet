using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal
{
    public class RESTAPICallPreHandler : IAPICallPreHandler
    {
        private string authorizeToken;

        /// <summary>
        /// Authorization Token
        /// </summary>
        public string authorizationToken
        {
            get
            {
                return authorizeToken;
            }
            set
            {
                authorizeToken = value;
            }
        }

        private string requestIdentity;

        /// <summary>
        /// Idempotency Request Id
        /// </summary>
        public string requestId
        {
            private get
            {
                return requestIdentity;
            }
            set
            {
                requestIdentity = value;
            }
        }

        public string payload;

        /// <summary>
        /// 
        /// </summary>
        public string payLoad
        {
            private get
            {
                return payload;
            }
            set
            {
                payload = value;
            }
        }

        /// <summary>
        /// Dynamic configuration map
        /// </summary>
        private Dictionary<string, string> config;

        /// <summary>
        /// Optional headers map
        /// </summary>
        private Dictionary<string, string> headersMap;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public RESTAPICallPreHandler(Dictionary<string, string> config)
        {
            this.config = ConfigManager.getConfigWithDefaults(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="headersMap"></param>
        public RESTAPICallPreHandler(Dictionary<string, string> config, Dictionary<string, string> headersMap)
        {
            this.config = ConfigManager.getConfigWithDefaults(config);
            this.headersMap = (headersMap == null) ? new Dictionary<string, string>() : headersMap;
        }

        #region IAPICallPreHandler Members

        public Dictionary<string, string> GetHeaderMap()
        {
            return GetProcessedHeadersMap();
        }

        public string GetPayLoad()
        {
            return GetProcessedPayload();
        }

        public string GetEndPoint()
        {
            return GetProcessedEndPoint();
        }

        public PayPal.Authentication.ICredential GetCredential()
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Overrided this method to return HTTP headers
        /// </summary>
        /// <returns>HTTP headers as Dictionary</returns>
        protected Dictionary<string, string> GetProcessedHeadersMap()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();

            /*
		     * The implementation is PayPal specific. The Authorization header is
		     * formed for OAuth or Basic, for OAuth system the authorization token
		     * passed as a parameter is used in creation of HTTP header, for Basic
		     * Authorization the ClientID and ClientSecret passed as parameters are
		     * used after a Base64 encoding.
		     */
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                headers.Add(BaseConstants.AUTHORIZATION_HEADER, authorizationToken);
            }
            else if (!string.IsNullOrEmpty(GetClientID()) && !string.IsNullOrEmpty(GetClientSecret()))
            {
                headers.Add(BaseConstants.AUTHORIZATION_HEADER, "Basic " + EncodeToBase64(GetClientID(), GetClientSecret()));
            }

            /*
             * Appends request Id which is used by PayPal API service for
		     * Idempotency
             */
            if (!string.IsNullOrEmpty(requestId))
            {
                headers.Add(BaseConstants.PAYPAL_REQUEST_ID_HEADER, requestId);
            }

            // Add User-Agent header for tracking in PayPal system
            Dictionary<string, string> userAgentMap = FormUserAgentHeader();
            if (userAgentMap != null && userAgentMap.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in userAgentMap)
                {
                    headers.Add(entry.Key, entry.Value);
                }
            }

            // Add any custom headers
            if (headersMap != null && headersMap.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in headersMap)
                {
                    headers.Add(entry.Key, entry.Value);
                }
            }
            return headers;
        }

        /// <summary>
        /// Override this method to post process the payload.
        /// The payload is returned unaltered as a default
        /// behaviour
        /// </summary>
        /// <returns>Payload string</returns>
        protected string GetProcessedPayload()
        {
            /*
		     * Since the REST API of PayPal depends on json, which is
		     * well formed, no additional processing is required.
		     */
            return payLoad;
        }

        /// <summary>
        /// Override this method to return default behavior for endpoint fetching
        /// </summary>
        /// <returns>Endpoint as a string</returns>
        protected string GetProcessedEndPoint()
        {
            string endpoint = null;
            if (config.ContainsKey(BaseConstants.END_POINT_CONFIG))
            {
                endpoint = config[BaseConstants.END_POINT_CONFIG];
            }
            else if (config.ContainsKey(BaseConstants.APPLICATION_MODE_CONFIG))
            {
                switch (config[BaseConstants.APPLICATION_MODE_CONFIG])
                {
                    case BaseConstants.LIVE_MODE:
                        endpoint = BaseConstants.REST_LIVE_ENDPOINT;
                        break;
                    case BaseConstants.SANDBOX_MODE:
                        endpoint = BaseConstants.REST_SANDBOX_ENDPOINT;
                        break;
                }
            }
            return endpoint;
        }

        /// <summary>
        /// Override this method to customize User-Agent header value
        /// </summary>
        /// <returns>User-Agent header value string</returns>
        protected Dictionary<string, string> FormUserAgentHeader()
        {
            UserAgentHeader userAgentHeader = new UserAgentHeader(
                PayPalResource.SdkID, PayPalResource.SdkVersion);
            return userAgentHeader.GetHeader();
        }

        private String GetClientID()
        {
            return this.config.ContainsKey(BaseConstants.CLIENT_ID) ? this.config[BaseConstants.CLIENT_ID] : null;
        }

        private String GetClientSecret()
        {
            return this.config.ContainsKey(BaseConstants.CLIENT_SECRET) ? this.config[BaseConstants.CLIENT_SECRET] : null;
        }

        private String EncodeToBase64(string clientID, string clientSecret)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(clientID + ":" + clientSecret);
                string base64ClientID = Convert.ToBase64String(bytes);
                return base64ClientID;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
        }
       
    }
}
