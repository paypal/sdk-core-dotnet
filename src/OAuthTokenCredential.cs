using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
/* NuGet Install
 * Visual Studio 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from the folder "net35"
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using PayPal.Manager;

namespace PayPal
{
    public class OAuthTokenCredential
    {
        private const string OAuthTokenPath = "/v1/oauth2/token";

        /// <summary>
        /// Client ID for OAuth
        /// </summary>
        private string clientID;

        /// <summary>
        /// Client Secret for OAuth
        /// </summary>
        private string clientSecret;

        /// <summary>
        /// Access Token that is generated
        /// </summary>
        private string accessToken;

        /// <summary>
        /// Application ID returned by OAuth servers
        /// </summary>
        private string appID;

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
        private static ILog logger = LogManagerWrapper.GetLogger(typeof(OAuthTokenCredential));

        /// <summary>
        /// Client ID and Secret for the OAuth
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientID, string clientSecret)
        {
            this.clientID = clientID;
            this.clientSecret = clientSecret;
            this.config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        /// <summary>
        /// Client ID and Secret for the OAuth
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientID, string clientSecret, Dictionary<string, string> config)
        {
            this.clientID = clientID;
            this.clientSecret = clientSecret;
            if (config != null)
            {
                ConfigManager.GetConfigWithDefaults(config);
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
            string base64ClientID = GenerateBase64String(clientID + ":" + clientSecret);
            generatedToken = GenerateOAuthToken(base64ClientID);
            return generatedToken;
        }

        private string GenerateBase64String(string clientCredential)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(clientCredential);
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

        private string GenerateOAuthToken(string base64ClientID)
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
            headers.Add("Authorization", "Basic " + base64ClientID);
            string postRequest = "grant_type=client_credentials";
            httpRequest.Method = "POST";
            httpRequest.Accept = "*/*";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.UserAgent = RESTConfiguration.FormUserAgentHeader();
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }

            HttpConnection httpConnection = new HttpConnection(config);
            response = httpConnection.Execute(postRequest, httpRequest);
            JObject deserializedObject = (JObject)JsonConvert.DeserializeObject(response);
            string generatedToken = (string)deserializedObject["token_type"] + " " + (string)deserializedObject["access_token"];
            appID = (string)deserializedObject["app_id"];
            aecondsToExpire = (int)deserializedObject["expires_in"];
            timeInMilliseconds = DateTime.Now.Millisecond;
            return generatedToken;
        }
    }    
}
