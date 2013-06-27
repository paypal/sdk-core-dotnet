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
        private string ClientID;

        /// <summary>
        /// Client Secret for OAuth
        /// </summary>
        private string ClientSecret;

        /// <summary>
        /// Access Token that is generated
        /// </summary>
        private string AccessToken;

        /// <summary>
        /// Application ID returned by OAuth servers
        /// </summary>
        private string AppID;

        /// <summary>
        /// Seconds for with access token is valid
        /// </summary>
        private int SecondsToExpire;

        /// <summary>
        /// Last time when access token was generated
        /// </summary>
        private long TimeInMilliseconds;

        /// <summary>
        /// Dynamic configuration map
        /// </summary>
        private Dictionary<string, string> Config;

        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static readonly ILog Logger = LogManagerWrapper.GetLogger(typeof(OAuthTokenCredential));

        /// <summary>
        /// Client ID and Secret for the OAuth
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientID, string clientSecret)
        {
            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            this.Config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        /// <summary>
        /// Client ID and Secret for the OAuth
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientID, string clientSecret, Dictionary<string, string> config)
        {
            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            if (config != null)
            {
                ConfigManager.GetConfigWithDefaults(config);
            }
            else
            {
                this.Config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
        }

        public string GetAccessToken()
        {
            // If Access Token is not Null and time has lapsed
            if (AccessToken != null)
            {
                // If the token has not expired
                // Set TTL as expiresTime - 60000
                // If expired set accesstoken == null
                if (((DateTime.Now.Millisecond - TimeInMilliseconds) / 1000) > (SecondsToExpire - 120))
                {
                    // regenerate token
                    AccessToken = null;
                }
            }
            // If accessToken is Null, Compute it
            if (AccessToken == null)
            {
                // Write Logic for passing in Detail to Identity Api Serv and
                // computing the token
                // Set the Value inside the accessToken and result
                AccessToken = GenerateAccessToken();
            }
            return AccessToken;
        }

        private string GenerateAccessToken()
        {
            string generatedToken = null;
            string base64ClientID = GenerateBase64String(ClientID + ":" + ClientSecret);
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
            if (Config.ContainsKey(BaseConstants.OAuthEndpoint))
            {
                baseUri = new Uri(Config[BaseConstants.OAuthEndpoint]);
            }
            else if (Config.ContainsKey(BaseConstants.EndpointConfig))
            {
                baseUri = new Uri(Config[BaseConstants.EndpointConfig]);
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

            HttpConnection httpConnection = new HttpConnection(Config);
            response = httpConnection.Execute(postRequest, httpRequest);
            JObject deserializedObject = (JObject)JsonConvert.DeserializeObject(response);
            string generatedToken = (string)deserializedObject["token_type"] + " " + (string)deserializedObject["access_token"];
            AppID = (string)deserializedObject["app_id"];
            SecondsToExpire = (int)deserializedObject["expires_in"];
            TimeInMilliseconds = DateTime.Now.Millisecond;
            return generatedToken;
        }
    }    
}
