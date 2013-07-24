using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from "net20" for Visual Studio 2005 or "net35" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Exception;
using PayPal.Manager;
using PayPal.Log;

namespace PayPal
{
    /// <summary>
    /// OAuthTokenCredential is used for generation of OAuth Token used by PayPal
    /// REST API service. clientId and clientSecret are required by the class to
    /// generate OAuth Token, the resulting token is of the form "Bearer xxxxxx". The
    /// class has two constructors, one of it taking an additional Dictionary
    /// used for dynamic configuration.
    /// </summary>
    public class OAuthTokenCredential
    {
        private const string OAuthTokenPath = "/v1/oauth2/token";

        /// <summary>
        /// Client Id for OAuth
        /// </summary>
        private string clientId;

        /// <summary>
        /// Client Secret for OAuth
        /// </summary>
        private string clientSecret;

        /// <summary>
        /// Access Token that is generated
        /// </summary>
        private string accessToken;

        /// <summary>
        /// Application Id returned by OAuth servers
        /// </summary>
        private string appId;

        /// <summary>
        /// Seconds for with access token is valid
        /// </summary>
        private int aecondsToExpire;

        /// <summary>
        /// Last time when access token was generated
        /// </summary>
        private long timeInMilliseconds;

        /// <summary>
        /// Dynamic configuration map
        /// </summary>
        private Dictionary<string, string> config;

        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static Log4netLogger logger = Log4netLogger.GetLogger(typeof(OAuthTokenCredential));

        /// <summary>
        /// Client Id and Secret for the OAuth
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        /// <summary>
        /// Client Id and Secret for the OAuth
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientId, string clientSecret, Dictionary<string, string> config)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            if (config != null)
            {
                this.config = ConfigManager.GetConfigWithDefaults(config);
            }
            else
            {
                this.config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
        }

        public string GetAccessToken()
        {
            // If Access Token is not Null and time has lapsed
            if (accessToken != null)
            {
                // If the token has not expired
                // Set TTL as expiresTime - 60000
                // If expired set accesstoken == null
                if (((DateTime.Now.Millisecond - timeInMilliseconds) / 1000) > (aecondsToExpire - 120))
                {
                    // regenerate token
                    accessToken = null;
                }
            }
            // If accessToken is Null, Compute it
            if (accessToken == null)
            {
                // Write Logic for passing in Detail to Identity Api Serv and
                // computing the token
                // Set the Value inside the accessToken and result
                accessToken = GenerateAccessToken();
            }
            return accessToken;
        }

        private string GenerateAccessToken()
        {
            string generatedToken = null;
            string base64ClientId = GenerateBase64String(clientId + ":" + clientSecret);
            generatedToken = GenerateOAuthToken(base64ClientId);
            return generatedToken;
        }

        private string GenerateBase64String(string clientCredential)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(clientCredential);
                string base64ClientId = Convert.ToBase64String(bytes);
                return base64ClientId;
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

        private string GenerateOAuthToken(string base64ClientId)
        {
            string response = null;

            Uri uniformResourceIdentifier = null;
            Uri baseUri = null;
            if (config.ContainsKey(BaseConstants.OAuthEndpoint))
            {
                baseUri = new Uri(config[BaseConstants.OAuthEndpoint]);
            }
            else if (config.ContainsKey(BaseConstants.EndpointConfig))
            {
                baseUri = new Uri(config[BaseConstants.EndpointConfig]);
            }
            bool success = Uri.TryCreate(baseUri, OAuthTokenPath, out uniformResourceIdentifier);
            ConnectionManager connManager = ConnectionManager.Instance;
            HttpWebRequest httpRequest = connManager.GetConnection(ConfigManager.Instance.GetProperties(), uniformResourceIdentifier.AbsoluteUri);
            
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Basic " + base64ClientId);
            string postRequest = "grant_type=client_credentials";
            httpRequest.Method = "POST";
            httpRequest.Accept = "*/*";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            UserAgentHeader userAgentHeader = new UserAgentHeader(PayPalResource.SDKId, PayPalResource.SDKVersion);
            Dictionary<string, string> userAgentMap = userAgentHeader.GetHeader();
            foreach (KeyValuePair<string, string> entry in userAgentMap)
            {
                httpRequest.UserAgent = entry.Value;
            }
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }

            HttpConnection httpConnection = new HttpConnection(config);
            response = httpConnection.Execute(postRequest, httpRequest);
            JObject deserializedObject = (JObject)JsonConvert.DeserializeObject(response);
            string generatedToken = (string)deserializedObject["token_type"] + " " + (string)deserializedObject["access_token"];
            appId = (string)deserializedObject["app_id"];
            aecondsToExpire = (int)deserializedObject["expires_in"];
            timeInMilliseconds = DateTime.Now.Millisecond;
            return generatedToken;
        }
    }    
}
