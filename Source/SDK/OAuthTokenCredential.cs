/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from "net20" for Visual Studio 2005 or "net35" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
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
        /// <summary>
        /// Specifies the PayPal endpoint for sending an OAuth request.
        /// </summary>
        private const string OAuthTokenPath = "/v1/oauth2/token";

        /// <summary>
        /// Dynamic configuration map
        /// </summary>
        private Dictionary<string, string> config;

        /// <summary>
        /// Cached access token that is generated when calling <see cref="OAuthtokenCredential.GetAccessToken()"/>.
        /// </summary>
        private string accessToken;

        /// <summary>
        /// SDKVersion instance
        /// </summary>
        private SDKVersion SdkVersion;

        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(OAuthTokenCredential));

        /// <summary>
        /// Gets the client ID to be used when creating an OAuth token.
        /// </summary>
        public string ClientId
        {
            get { return this._clientId; }
            private set { this._clientId = value; }
        }
        private string _clientId;

        /// <summary>
        /// Gets the client secret to be used when creating an OAuth token.
        /// </summary>
        public string ClientSecret
        {
            get { return this._clientSecret; }
            private set { this._clientSecret = value; }
        }
        private string _clientSecret;

        /// <summary>
        /// Gets the application ID returned by OAuth servers.
        /// Must first call <see cref="OAuthtokenCredentials.GetAccessToken()"/> to populate this property.
        /// </summary>
        public string ApplicationId
        {
            get { return this._applicationId; }
            private set { this._applicationId = value; }
        }
        private string _applicationId;

        /// <summary>
        /// Gets or sets the lifetime of a created access token in seconds.
        /// Must first call <see cref="OAuthtokenCredentials.GetAccessToken()"/> to populate this property.
        /// </summary>
        public int AccessTokenExpirationInSeconds
        {
            get { return this._accessTokenExpirationInSeconds; }
            set { this._accessTokenExpirationInSeconds = value; }
        }
        private int _accessTokenExpirationInSeconds;

        /// <summary>
        /// Gets the last date when access token was generated.
        /// Must first call <see cref="OAuthtokenCredentials.GetAccessToken()"/> to populate this property.
        /// </summary>
        public DateTime AccessTokenLastCreationDate
        {
            get { return this._accessTokenLastCreationDate; }
            private set { this._accessTokenLastCreationDate = value; }
        }
        private DateTime _accessTokenLastCreationDate;

        /// <summary>
        /// Gets or sets the safety gap when checking the expiration of an already created access token in seconds.
        /// If the elapsed time since the last access token was created is more than the expiration - the safety gap,
        /// then a new token will be created when calling <see cref="OAuthTokenCredential.GetAccessToken()"/>.
        /// </summary>
        public int AccessTokenExpirationSafetyGapInSeconds
        {
            get { return this._accessTokenExpirationSafetyGapInSeconds; }
            set { this._accessTokenExpirationSafetyGapInSeconds = value; }
        }
        private int _accessTokenExpirationSafetyGapInSeconds;
               
        /// <summary>
        /// Client Id and Secret for the OAuth
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientId, string clientSecret) : this(clientId, clientSecret, null)
        {
        }

        /// <summary>
        /// Client Id and Secret for the OAuth
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public OAuthTokenCredential(string clientId, string clientSecret, Dictionary<string, string> config)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.config = config != null ? ConfigManager.GetConfigWithDefaults(config) : ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties()); 
            this.SdkVersion = new SDKVersionImpl();
            this.AccessTokenExpirationSafetyGapInSeconds = 120; // Default is 2 minute safety gap for token expiration.
        }

        /// <summary>
        /// Returns the currently cached access token. If no access token was
        /// previously cached, or if the current access token is expired, then
        /// a new one is generated and returned.
        /// </summary>
        /// <returns>The OAuth access token to use for making PayPal requests.</returns>
        /// <exception cref="PayPal.Exception.MissingCredentialException">Thrown if clientId or clientSecret are null or empty.</exception>
        /// <exception cref="PayPal.Exception.InvalidCredentialException">Thrown if there is an issue converting the credentials to a formatted authorization string.</exception>
        /// <exception cref="PayPal.Exception.IdentityException">Thrown if authorization fails as a result of providing invalid credentials.</exception>
        /// <exception cref="PayPal.Exception.HttpException">Thrown if authorization fails and an HTTP error response is received.</exception>
        /// <exception cref="PayPal.Exception.ConnectionException">Thrown if there is an issue attempting to connect to PayPal's services.</exception>
        /// <exception cref="PayPal.Exception.ConfigException">Thrown if there is an error with any informaiton provided by the <see cref="PayPal.Manager.ConfigManager"/>.</exception>
        /// <exception cref="PayPal.Exception.PayPalException">Thrown for any other general exception. See inner exception for further details.</exception>
        public string GetAccessToken()
        {
            // If the cached access token value is valid, then check to see if
            // it has expired.
            if (!string.IsNullOrEmpty(this.accessToken))
            {
                // If the time since the access token was created is greater
                // than the access token's specified expiration time less the
                // safety gap, then regenerate the token.
                double elapsedSeconds = (DateTime.Now - this.AccessTokenLastCreationDate).TotalSeconds;
                if (elapsedSeconds > this.AccessTokenExpirationInSeconds - this.AccessTokenExpirationSafetyGapInSeconds)
                {
                    this.accessToken = null;
                }
            }

            // If the cached access token is empty or null, then generate a new token.
            if (string.IsNullOrEmpty(this.accessToken))
            {
                // Write Logic for passing in Detail to Identity Api Serv and
                // computing the token
                // Set the Value inside the accessToken and result
                string base64ClientId = OAuthTokenCredential.ConvertClientCredentialsToBase64String(this.ClientId, this.ClientSecret);
                this.accessToken = this.GenerateOAuthToken(base64ClientId);
            }
            return this.accessToken;
        }

        /// <summary>
        /// Covnerts the specified client credentials to a base-64 string for authorization purposes.
        /// </summary>
        /// <param name="clientId">The client ID to be used in generating the base-64 client identifier.</param>
        /// <param name="clientSecret">The client secret to be used in generating the base-64 client identifier.</param>
        /// <returns>The base-64 encoded client identifier to use in the authorization request.</returns>
        /// <exception cref="PayPal.Exception.MissingCredentialException">Thrown if clientId or clientSecret are null or empty.</exception>
        /// <exception cref="PayPal.Exception.InvalidCredentialException">Thrown if there is an issue converting the credentials to a formatted authorization string.</exception>
        /// <exception cref="PayPal.Exception.PayPalException">Thrown for any other issue encountered. See inner exception for further details.</exception>
        private static string ConvertClientCredentialsToBase64String(string clientId, string clientSecret)
        {
            // Validate the provided credentials. If either value is null or empty, then throw.
            if (string.IsNullOrEmpty(clientId))
            {
                throw new MissingCredentialException("clientId is missing.");
            }
            else if (string.IsNullOrEmpty(clientSecret))
            {
                throw new MissingCredentialException("clientSecret is missing.");
            }

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", clientId, clientSecret));
                return Convert.ToBase64String(bytes);
            }
            catch (System.Exception ex)
            {
                if (ex is FormatException || ex is ArgumentNullException)
                {
                    throw new InvalidCredentialException("Unable to convert client credentials to base-64 string.\n" +
                                                         "  clientId: \"" + clientId + "\"\n" +
                                                         "  clientSecret: \"" + clientSecret + "\"\n" +
                                                         "  Error: " + ex.Message);
                }

                throw new PayPalException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generates a new OAuth token useing the specified client credentials in the authorization request.
        /// </summary>
        /// <param name="base64ClientId">The base-64 encoded credentials to be used in the authorization request.</param>
        /// <returns>The OAuth access token to use for making PayPal requests.</returns>
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
                else if (mode.Equals(BaseConstants.TestSandboxMode))
                {
                    baseUri = new Uri(BaseConstants.RESTTestSandboxEndpoint);
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
                Encoding iso8851 = Encoding.GetEncoding("iso-8859-1", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
                byte[] bytes = Encoding.Convert(Encoding.UTF8,iso8851, Encoding.UTF8.GetBytes(entry.Value));                
                httpRequest.UserAgent = iso8851.GetString(bytes);
            }
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }

            HttpConnection httpConnection = new HttpConnection(config);
            try
            {
                response = httpConnection.Execute(postRequest, httpRequest);
            }
            catch (PayPal.Exception.HttpException ex)
            {
                // If we get an HTTP exception back and the status code is 401,
                // then try and generate an IdentityException object to throw.
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    IdentityException identityEx;
                    if (ex.TryConvertTo<IdentityException>(out identityEx))
                    {
                        throw identityEx;
                    }
                }

                throw;
            }

            JObject deserializedObject = (JObject)JsonConvert.DeserializeObject(response);
            string generatedToken = (string)deserializedObject["token_type"] + " " + (string)deserializedObject["access_token"];
            this.ApplicationId = (string)deserializedObject["app_id"];
            this.AccessTokenExpirationInSeconds = (int)deserializedObject["expires_in"];
            this.AccessTokenLastCreationDate = DateTime.Now;
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
