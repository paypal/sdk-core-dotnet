using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
// NuGet Install
// install PayPalCoreSDK -excludeversion -outputDirectory .\Packages
// 2.0
using log4net;
using PayPal.Exception;
using PayPal.Manager;
// install Newtonsoft.Json -excludeversion -outputDirectory .\Packages
// net35
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PayPal
{
    public class OAuthTokenCredential
    {

        private const string OAUTHTOKENPATH = "/v1/oauth2/token";
        /// <summary>
        /// Client ID for OAuth
        /// </summary>
        private String clientID;

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
        private int secondsToExpire;

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
        private static readonly ILog logger = LogManagerWrapper.GetLogger(typeof(OAuthTokenCredential));
               
        /// <summary>
        /// Client ID and Secret for the OAuth
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(String clientID, String clientSecret)
        {
            this.clientID = clientID;
            this.clientSecret = clientSecret;
            this.config = ConfigManager.getConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        /// <summary>
        /// Client ID and Secret for the OAuth
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(String clientID, String clientSecret, Dictionary<string, string> config)
        {
            this.clientID = clientID;
            this.clientSecret = clientSecret;
            this.config = config != null ? ConfigManager.getConfigWithDefaults(config): ConfigManager.getConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        public string GetAccessToken()
        {
            // If Access Token is not Null and time has lapsed
            if (accessToken != null)
            {
                // If the token has not expired
                // Set TTL as expiresTime - 60000
                // If expired set accesstoken == null
                if (((DateTime.Now.Millisecond - timeInMilliseconds) / 1000) > (secondsToExpire - 120))
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
                if (config.ContainsKey(BaseConstants.OAUTH_ENDPOINT))
                {
                    baseUri = new Uri(config[BaseConstants.OAUTH_ENDPOINT]);
                }
                else if (config.ContainsKey(BaseConstants.END_POINT_CONFIG))
                {
                    baseUri = new Uri(config[BaseConstants.END_POINT_CONFIG]);
                }
                else if (config.ContainsKey(BaseConstants.APPLICATION_MODE_CONFIG))
                {
                    string mode = config[BaseConstants.APPLICATION_MODE_CONFIG];
                    if (mode.Equals(BaseConstants.LIVE_MODE))
                    {
                        baseUri = new Uri(BaseConstants.REST_LIVE_ENDPOINT);
                    }
                    else if (mode.Equals(BaseConstants.SANDBOX_MODE))
                    {
                        baseUri = new Uri(BaseConstants.REST_SANDBOX_ENDPOINT);
                    }
                    else
                    {
                        throw new ConfigException("You must specify one of mode(live/sandbox) OR endpoint in the configuration");
                    }
                }
                bool success = Uri.TryCreate(baseUri, OAUTHTOKENPATH, out uniformResourceIdentifier);
                ConnectionManager connManager = ConnectionManager.Instance;
                HttpWebRequest httpRequest = connManager.GetConnection(this.config, uniformResourceIdentifier.AbsoluteUri);  
              
                
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
                secondsToExpire = (int)deserializedObject["expires_in"];
                timeInMilliseconds = DateTime.Now.Millisecond;
                return generatedToken;
        }
    }
}
