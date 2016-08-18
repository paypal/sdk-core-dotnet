using System.Collections.Generic;
using System.Text;
using System.Web;
using PayPal.Manager;

namespace PayPal.OpenIdConnect
{
    public class Session
    {
        /// <summary>
        /// Returns the PayPal URL to which the user must be redirected to start the authentication / authorization process
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="scope"></param>
        /// <param name="apiContext"></param>
        /// <returns></returns>
        public static string GetRedirectUrl(string redirectUri, List<string> scope, APIContext apiContext)
        {
            string redirectUrl = null;
            string baseUrl = null;
            Dictionary<string, string> config = null;

            if (apiContext.Config == null)
            {
                config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
            else
            {
                config = ConfigManager.GetConfigWithDefaults(apiContext.Config);
            }
            
            if (config.ContainsKey(BaseConstants.OpenIdRedirectUri))
            {
                baseUrl = config[BaseConstants.OpenIdRedirectUri];
            }
            else
            {
                baseUrl = BaseConstants.OpenIdRedirectUriConstant;
            }
            if (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            if (scope == null || scope.Count <= 0)
            {
                scope = new List<string>();
                scope.Add("openid");
                scope.Add("profile");
                scope.Add("address");
                scope.Add("email");
                scope.Add("phone");
                scope.Add("https://uri.paypal.com/services/paypalattributes");
            }
            if (!scope.Contains("openid"))
            {
                scope.Add("openid");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("client_id=").Append(HttpUtility.UrlEncode((config.ContainsKey(BaseConstants.ClientId)) ? config[BaseConstants.ClientId] : string.Empty)).Append("&response_type=").Append("code").Append("&scope=");
            StringBuilder scpBuilder = new StringBuilder();
            foreach (string str in scope)
            {
                scpBuilder.Append(str).Append(" ");
            }
            builder.Append(HttpUtility.UrlEncode(scpBuilder.ToString()));
            builder.Append("&redirect_uri=").Append(HttpUtility.UrlEncode(redirectUri));
            redirectUrl = baseUrl + "/signin/authorize?" + builder.ToString();
            return redirectUrl;
        }

        /// <summary>
        /// Returns the URL to which the user must be redirected to logout from the OpenId provider (i.e., PayPal)
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="idToken"></param>
        /// <param name="apiContext"></param>
        /// <returns></returns>
        public static string GetLogoutUrl(string redirectUri, string idToken, APIContext apiContext)
        {
            string logoutUrl = null;
            string baseUrl = null;
            Dictionary<string, string> config = null;

            if (apiContext.Config == null)
            {
                config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
            else
            {
                config = ConfigManager.GetConfigWithDefaults(apiContext.Config);
            }
            
            if (config.ContainsKey(BaseConstants.OpenIdRedirectUri))
            {
                baseUrl = config[BaseConstants.OpenIdRedirectUri];
            }
            else
            {
                baseUrl = BaseConstants.OpenIdRedirectUriConstant;
            }
            if (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("id_token=")
                    .Append(HttpUtility.UrlEncode(idToken))
                    .Append("&redirect_uri=")
                    .Append(HttpUtility.UrlEncode(redirectUri))
                    .Append("&logout=true");
            logoutUrl = baseUrl + "/webapps/auth/protocol/openidconnect/v1/endsession?" + stringBuilder.ToString();
            return logoutUrl;
        }
    }
}
