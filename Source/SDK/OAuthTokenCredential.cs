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
using System.Web;

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
        /// SDKVersion instance
        /// </summary>
        private SDKVersion SdkVersion;

        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(OAuthTokenCredential));
               
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
            this.SdkVersion = new SDKVersionImpl();
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
            this.config = config != null ? ConfigManager.GetConfigWithDefaults(config) : ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties()); 
            this.SdkVersion = new SDKVersionImpl();
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
            else if (config.ContainsKey(BaseConstants.ApplicationModeConfig))
            {
                string mode = config[BaseConstants.ApplicationModeConfig];
                if (mode.Equals(BaseConstants.LiveMode))
                {
                    baseUri = new Uri(BaseConstants.RESTLiveEndpoint);
                }
                else if (mode.Equals(BaseConstants.SandboxMode))
                {
                    baseUri = new Uri(BaseConstants.RESTSandboxEndpoint);
                }
                else
                {
                    throw new ConfigException("You must specify one of mode(live/sandbox) OR endpoint in the configuration");
                }
            }
            bool success = Uri.TryCreate(baseUri, OAuthTokenPath, out uniformResourceIdentifier);
            ConnectionManager connManager = ConnectionManager.Instance;
                HttpWebRequest httpRequest = connManager.GetConnection(this.config, uniformResourceIdentifier.AbsoluteUri);  
            
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Basic " + base64ClientId);
            string postRequest = "grant_type=client_credentials";
            httpRequest.Method = "POST";
            httpRequest.Accept = "*/*";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            UserAgentHeader userAgentHeader = new UserAgentHeader(SdkVersion == null ? "" : SdkVersion.GetSDKId(), SdkVersion == null ? "" : SdkVersion.GetSDKVersion());
            Dictionary<string, string> userAgentMap = userAgentHeader.GetHeader();
            foreach (KeyValuePair<string, string> entry in userAgentMap)
            {
                // aganzha
                //iso-8859-1
                var iso8851 = Encoding.GetEncoding("iso-8859-1", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
                var bytes = Encoding.Convert(Encoding.UTF8,iso8851, Encoding.UTF8.GetBytes(entry.Value));                
                httpRequest.UserAgent = iso8851.GetString(bytes);
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
            secondsToExpire = (int)deserializedObject["expires_in"];
            timeInMilliseconds = DateTime.Now.Millisecond;
            return generatedToken;
        }

        private class SDKVersionImpl : SDKVersion
        {

            public string GetSDKId()
            {
                return BaseConstants.SdkId;
            }

            public string GetSDKVersion()
            {
                return BaseConstants.SdkVersion;
            }
        }
    }

    
}
