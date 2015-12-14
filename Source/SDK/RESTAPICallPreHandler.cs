using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal
{
    /// <summary>
    /// RESTApiCallPreHandler requires a configuration system to function properly. Pass
    /// a config Dictionary for dynamic configuration.
    /// </summary>
    public class RESTAPICallPreHandler : IAPICallPreHandler
    {
        /// <summary>
        /// string Authorization Token
        /// </summary>
        private string authorizeToken;

        /// <summary>
        /// Idempotency Request Id
        /// </summary>
        private string reqId;

        /// <summary>
        /// Dynamic configuration map
        /// </summary>
        private Dictionary<string, string> config;

        /// <summary>
        /// SDKVersion instance
        /// </summary>
        private SDKVersion sVersion;

        /// <summary>
        ///  Gets and sets the Authorization Token
        /// </summary>
        public string AuthorizationToken
        {
            get
            {
                return this.authorizeToken;
            }
            set
            {
                this.authorizeToken = value;
            }
        }

        /// <summary>
        /// Gets and sets the Idempotency Request Id
        /// </summary>
        public string RequestId
        {
            private get
            {
                return reqId;
            }
            set
            {
                reqId = value;
            }
        }

        public string pLoad;

        /// <summary>
        /// Payload
        /// </summary>
        public string Payload
        {
            get
            {
                return pLoad;
            }
            set
            {
                pLoad = value;
            }
        }

        public SDKVersion SdkVersion
        {
            get
            {
                return sVersion;
            }
            set
            {
                sVersion = value;
            }
        }

        /// <summary>
        /// Optional headers map
        /// </summary>
        private Dictionary<string, string> headersMap;

        /// <summary>
        /// RESTAPICallPreHandler taking dynamic configuration Dictionary
        /// </summary>
        /// <param name="config">Dictionary for dynamic configuration</param>
        public RESTAPICallPreHandler(Dictionary<string, string> config)
        {
            this.config = ConfigManager.GetConfigWithDefaults(config);
        }

        /// <summary>
        /// RESTAPICallPreHandler taking dynamic configuration Dictionary and HTTP Headers Dictionary
        /// </summary>
        /// <param name="config">Dictionary for dynamic configuration</param>
        /// <param name="headersMap">Dictionary for HTTP Headers</param>
        public RESTAPICallPreHandler(Dictionary<string, string> config, Dictionary<string, string> headersMap)
        {
            this.config = ConfigManager.GetConfigWithDefaults(config);
            this.headersMap = (headersMap == null) ? new Dictionary<string, string>() : headersMap;
        }

        public Dictionary<string, string> GetHeaderMap()
        {
            return GetProcessedHeadersMap();
        }

        public string GetPayload()
        {
            return GetProcessedPayload();
        }

        public string GetEndpoint()
        {
            return GetProcessedEndPoint();
        }

        public PayPal.Authentication.ICredential GetCredential()
        {
            return null;
        }

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
            if (!string.IsNullOrEmpty(AuthorizationToken))
            {
                headers.Add(BaseConstants.AuthorizationHeader, AuthorizationToken);
            }
            else if (!string.IsNullOrEmpty(GetClientID()) && !string.IsNullOrEmpty(GetClientSecret()))
            {
                headers.Add(BaseConstants.AuthorizationHeader, "Basic " + EncodeToBase64(GetClientID(), GetClientSecret()));
            }

            /*
             * Appends request Id which is used by PayPal API service for
		     * Idempotency
             */
            if (!string.IsNullOrEmpty(RequestId))
            {
                headers.Add(BaseConstants.PayPalRequestIdHeader, RequestId);
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
            return Payload;
        }

        /// <summary>
        /// Override this method to return default behavior for endpoint fetching
        /// </summary>
        /// <returns>Endpoint as a string</returns>
        protected string GetProcessedEndPoint()
        {
            string endpoint = null;
            if (config.ContainsKey(BaseConstants.EndpointConfig))
            {
                endpoint = config[BaseConstants.EndpointConfig];
            }
            else if (config.ContainsKey(BaseConstants.ApplicationModeConfig))
            {
                switch (config[BaseConstants.ApplicationModeConfig])
                {
                    case BaseConstants.LiveMode:
                        endpoint = BaseConstants.RESTLiveEndpoint;
                        break;
                    case BaseConstants.SandboxMode:
                        endpoint = BaseConstants.RESTSandboxEndpoint;
                        break;
                    case BaseConstants.TestSandboxMode:
                        endpoint = BaseConstants.RESTTestSandboxEndpoint;
                        break;
                }
            }
            if (!endpoint.EndsWith("/"))
            {
                endpoint += "/";
            }
            return endpoint;
        }

        /// <summary>
        /// Override this method to customize User-Agent header value
        /// </summary>
        /// <returns>User-Agent header value string</returns>
        protected Dictionary<string, string> FormUserAgentHeader()
        {
            UserAgentHeader userAgentHeader = new UserAgentHeader((SdkVersion == null)? null : SdkVersion.GetSDKId(), (SdkVersion == null)? null : SdkVersion.GetSDKVersion());
            return userAgentHeader.GetHeader();
        }

        private String GetClientID()
        {
            return this.config.ContainsKey(BaseConstants.ClientId) ? this.config[BaseConstants.ClientId] : null;
        }

        private String GetClientSecret()
        {
            return this.config.ContainsKey(BaseConstants.ClientSecret) ? this.config[BaseConstants.ClientSecret] : null;
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
