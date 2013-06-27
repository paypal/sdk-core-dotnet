using System.Web;
using System.Collections.Generic;
/* NuGet Install
 * Visual Studio 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from the folder "net35"
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayPal.Util;

namespace PayPal.OpenIDConnect
{
    public class Tokeninfo
    {
        /// <summary>
        /// OPTIONAL, if identical to the scope requested by the client; otherwise, REQUIRED
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scope
        {
            get;
            set;
        }

        /// <summary>
        /// The access token issued by the authorization server
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string access_token
        {
            get;
            set;
        }

        /// <summary>
        /// The refresh token, which can be used to obtain new access tokens using the same authorization grant as described in OAuth2.0 RFC6749 in Section 6
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string refresh_token
        {
            get;
            set;
        }

        /// <summary>
        /// The type of the token issued as described in OAuth2.0 RFC6749 (Section 7.1).  Value is case insensitive
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string token_type
        {
            get;
            set;
        }

        /// <summary>
        /// The lifetime in seconds of the access token
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int expires_in
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor overload
        /// </summary>
        public Tokeninfo(string access_token, string token_type, int expires_in)
        {
            this.access_token = access_token;
            this.token_type = token_type;
            this.expires_in = expires_in;
        }

        /// <summary>
        /// Creates an Access Token from an Authorization Code.
        /// <param name="createFromAuthorizationCodeParameters">Query parameters used for API call</param>
        /// </summary>
        public static Tokeninfo CreateFromAuthorizationCode(CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters)
        {
            string pattern = "v1/identity/openidconnect/tokenservice ?grant_type={0}&code={1}&redirect_uri={2}";
            object[] parameters = new object[] { createFromAuthorizationCodeParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = resourcePath.Substring(resourcePath.IndexOf('?') + 1);
            resourcePath = resourcePath.Substring(0, resourcePath.IndexOf("?"));
            Dictionary<string, string> headersMap = new Dictionary<string, string>();
            headersMap.Add("Content-Type", "application/x-www-form-urlencoded");
            return PayPalResource.ConfigureAndExecute<Tokeninfo>(null, HttpMethod.POST,
                    resourcePath, headersMap, payLoad);
        }

        /// <summary>
        /// Creates an Access Token from an Authorization Code.
        /// <param name="apiContext">APIContext to be used for the call.</param>
        /// <param name="createFromAuthorizationCodeParameters">Query parameters used for API call</param>
        /// </summary>
        public static Tokeninfo CreateFromAuthorizationCode(APIContext apiContext, CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters)
        {
            string pattern = "v1/identity/openidconnect/tokenservice ?grant_type={0}&code={1}&redirect_uri={2}";
            object[] parameters = new object[] { createFromAuthorizationCodeParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = resourcePath.Substring(resourcePath.IndexOf('?') + 1);
            resourcePath = resourcePath.Substring(0, resourcePath.IndexOf("?"));
            Dictionary<string, string> headersMap = new Dictionary<string, string>();
            headersMap.Add("Content-Type", "application/x-www-form-urlencoded");
            return PayPalResource.ConfigureAndExecute<Tokeninfo>(apiContext, HttpMethod.POST,
                    resourcePath, headersMap, payLoad);
        }

        /// <summary>
        /// Creates an Access Token from an Refresh Token.
        /// <param name="createFromRefreshTokenParameters">Query parameters used for API call</param>
        /// </summary>
        public Tokeninfo CreateFromRefreshToken(CreateFromRefreshTokenParameters createFromRefreshTokenParameters)
        {
            string pattern = "v1/identity/openidconnect/tokenservice ?grant_type={0}&refresh_token={1}&scope={2}&client_id={3}&client_secret={4}";
            createFromRefreshTokenParameters.SetRefreshToken(HttpUtility.UrlEncode(refresh_token));
            object[] parameters = new object[] { createFromRefreshTokenParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = resourcePath.Substring(resourcePath.IndexOf('?') + 1);
            resourcePath = resourcePath.Substring(0, resourcePath.IndexOf("?"));
            Dictionary<string, string> headersMap = new Dictionary<string, string>();
            headersMap.Add("Content-Type", "application/x-www-form-urlencoded");
            return PayPalResource.ConfigureAndExecute<Tokeninfo>(null, HttpMethod.POST,
                    resourcePath, headersMap, payLoad);
        }

        /// <summary>
        /// Creates an Access Token from an Refresh Token
        /// <param name="apiContext">APIContext to be used for the call</param>
        /// <param name="createFromRefreshTokenParameters">Query parameters used for API call</param>
        /// </summary>
        public Tokeninfo CreateFromRefreshToken(APIContext apiContext, CreateFromRefreshTokenParameters createFromRefreshTokenParameters)
        {
            string pattern = "v1/identity/openidconnect/tokenservice ?grant_type={0}&refresh_token={1}&scope={2}&client_id={3}&client_secret={4}";
            createFromRefreshTokenParameters.SetRefreshToken(HttpUtility.UrlEncode(refresh_token));
            object[] parameters = new object[] { createFromRefreshTokenParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = resourcePath.Substring(resourcePath.IndexOf('?') + 1);
            resourcePath = resourcePath.Substring(0, resourcePath.IndexOf("?"));
            Dictionary<string, string> headersMap = new Dictionary<string, string>();
            headersMap.Add("Content-Type", "application/x-www-form-urlencoded");
            return PayPalResource.ConfigureAndExecute<Tokeninfo>(apiContext,
                    HttpMethod.POST, resourcePath, headersMap, payLoad);
        }
    }
}



