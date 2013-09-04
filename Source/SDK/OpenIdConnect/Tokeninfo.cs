using System.Web;
using System.Collections.Generic;
/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from "net20" for Visual Studio 2005 or "net35" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayPal.Util;

namespace PayPal.OpenIdConnect
{
    public class Tokeninfo
    {
        /// <summary>
        /// OPTIONAL, if identical to the scope requested by the client otherwise, REQUIRED
        /// </summary>
        private string scopeRequested;

        /// <summary>
        /// The access token issued by the authorization server
        /// </summary>
        private string accessToken;

        /// <summary>
        /// The refresh token, which can be used to obtain new access tokens using the same authorization grant as described in OAuth2.0 RFC6749 in Section 6
        /// </summary>
        private string refreshToken;

        /// <summary>
        /// The type of the token issued as described in OAuth2.0 RFC6749 (Section 7.1), value is case insensitive
        /// </summary>
        private string tokenType;

        /// <summary>
        /// The lifetime in seconds of the access token
        /// </summary>
        private int expiresIn;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scope
        {
            get
            {
                return scopeRequested;
            }
            set
            {
                scopeRequested = value;
            }
        }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string access_token
        {
            get
            {
                return accessToken;
            }
            set
            {
                accessToken = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string refresh_token
        {
            get
            {
                return refreshToken;
            }
            set
            {
                refreshToken = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string token_type
        {
            get
            {
                return tokenType;
            }
            set
            {
                tokenType = value;
            }
        }
       
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int expires_in
        {
            get
            {
                return expiresIn;
            }
            set
            {
                expiresIn = value;
            }
        }

        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public Tokeninfo() { }

        /// <summary>
        /// Constructor overload
        /// </summary>
        public Tokeninfo(string accessToken, string tokenType, int expiresIn)
        {
            this.access_token = accessToken;
            this.token_type = tokenType;
            this.expires_in = expiresIn;
        }

        /// <summary>
        /// Creates an Access Token from an Authorization Code.
        /// <param name="createFromAuthorizationCodeParameters">Query parameters used for API call</param>
        /// </summary>
        public static Tokeninfo CreateFromAuthorizationCode(CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters)
        {
            return CreateFromAuthorizationCode(null, createFromAuthorizationCodeParameters);
        }

        /// <summary>
        /// Creates an Access Token from an Authorization Code.
        /// <param name="apiContext">APIContext to be used for the call.</param>
        /// <param name="createFromAuthorizationCodeParameters">Query parameters used for API call</param>
        /// </summary>
        public static Tokeninfo CreateFromAuthorizationCode(APIContext apiContext, CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters)
        {
            string pattern = "v1/identity/openidconnect/tokenservice?grant_type={0}&code={1}&redirect_uri={2}";
            object[] parameters = new object[] { createFromAuthorizationCodeParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = resourcePath.Substring(resourcePath.IndexOf('?') + 1);
            resourcePath = resourcePath.Substring(0, resourcePath.IndexOf("?"));
            Dictionary<string, string> headersMap = new Dictionary<string, string>();
            headersMap.Add(BaseConstants.ContentTypeHeader, "application/x-www-form-urlencoded");
            if (apiContext == null)
            {
                apiContext = new APIContext();
            }
            apiContext.HTTPHeaders = headersMap;
            apiContext.MaskRequestId = true;
            return PayPalResource.ConfigureAndExecute<Tokeninfo>(apiContext, HttpMethod.POST, resourcePath, payLoad);
        }

        /// <summary>
        /// Creates an Access Token from an Refresh Token.
        /// <param name="createFromRefreshTokenParameters">Query parameters used for API call</param>
        /// </summary>
        public Tokeninfo CreateFromRefreshToken(CreateFromRefreshTokenParameters createFromRefreshTokenParameters)
        {
            return CreateFromRefreshToken(null, createFromRefreshTokenParameters);
        }

        /// <summary>
        /// Creates an Access Token from an Refresh Token
        /// <param name="apiContext">APIContext to be used for the call</param>
        /// <param name="createFromRefreshTokenParameters">Query parameters used for API call</param>
        /// </summary>
        public Tokeninfo CreateFromRefreshToken(APIContext apiContext, CreateFromRefreshTokenParameters createFromRefreshTokenParameters)
        {
            string pattern = "v1/identity/openidconnect/tokenservice?grant_type={0}&refresh_token={1}&scope={2}&client_id={3}&client_secret={4}";
            createFromRefreshTokenParameters.SetRefreshToken(HttpUtility.UrlEncode(refresh_token));
            object[] parameters = new object[] { createFromRefreshTokenParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = resourcePath.Substring(resourcePath.IndexOf('?') + 1);
            resourcePath = resourcePath.Substring(0, resourcePath.IndexOf("?"));
            Dictionary<string, string> headersMap = new Dictionary<string, string>();
            headersMap.Add(BaseConstants.ContentTypeHeader, "application/x-www-form-urlencoded");
            if (apiContext == null)
            {
                apiContext = new APIContext();
            }
            apiContext.HTTPHeaders = headersMap;
            apiContext.MaskRequestId = true;
            return PayPalResource.ConfigureAndExecute<Tokeninfo>(apiContext, HttpMethod.POST, resourcePath, payLoad);
        }
    }
}



