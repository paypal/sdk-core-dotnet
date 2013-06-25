using System.Collections.Generic;
using System.Text;
using System.Web;
using PayPal.Manager;

namespace PayPal.OpenidConnect
{
    public class Session
    {

        /// <summary>
        /// Returns the PayPal URL to which the user must be redirected to start the 
        /// authentication / authorization process.
        /// </summary>
        /// <param name="redirectURI"></param>
        /// <param name="scope"></param>
        /// <param name="apiContext"></param>
        /// <returns></returns>
        public static string GetRedirectURL(string redirectURI, List<string> scope,
            APIContext apiContext)
        {
            string redirectURL = null;
            Dictionary<string, string> config = null;
            if (apiContext.Config == null)
            {
                config = ConfigManager.getConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
            else
            {
                config = ConfigManager.getConfigWithDefaults(apiContext.Config);
            }
            string baseURL = null;
            if (config.ContainsKey(BaseConstants.OPENID_REDIRECT_URI))
            {
                baseURL = config[BaseConstants.OPENID_REDIRECT_URI];
            }
            else
            {
                baseURL = BaseConstants.OPENID_REDIRECT_URI_CONSTANT;
            }
            if (baseURL.EndsWith("/"))
            {
                baseURL = baseURL.Substring(0, baseURL.Length - 1);
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
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("client_id=").Append(HttpUtility.UrlEncode((config.ContainsKey(BaseConstants.CLIENT_ID)) ? config[BaseConstants.CLIENT_ID] : string.Empty)).Append("&response_type=").Append("code").Append("&scope=");
            StringBuilder scpBuilder = new StringBuilder();
            foreach (string str in scope)
            {
                scpBuilder.Append(str).Append(" ");
            }
            strBuilder.Append(HttpUtility.UrlEncode(scpBuilder.ToString()));
            strBuilder.Append("&redirect_uri=").Append(
                    HttpUtility.UrlEncode(redirectURI));
            redirectURL = baseURL + "/v1/authorize?" + strBuilder.ToString();
            return redirectURL;
        }

        /// <summary>
        /// Returns the URL to which the user must be redirected to logout from the
        /// OpenID provider (i.e. PayPal)
        /// </summary>
        /// <param name="redirectURI"></param>
        /// <param name="idToken"></param>
        /// <param name="apiContext"></param>
        /// <returns></returns>
        public static string GetLogoutUrl(string redirectURI, string idToken,
            APIContext apiContext)
        {
            string logoutURL = null;
            Dictionary<string, string> config = null;
            if (apiContext.Config == null)
            {
                config = ConfigManager.getConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
            else
            {
                config = ConfigManager.getConfigWithDefaults(apiContext.Config);
            }
            string baseURL = null;
            if (config.ContainsKey(BaseConstants.OPENID_REDIRECT_URI))
            {
                baseURL = config[BaseConstants.OPENID_REDIRECT_URI];
            }
            else
            {
                baseURL = BaseConstants.OPENID_REDIRECT_URI_CONSTANT;
            }
            if (baseURL.EndsWith("/"))
            {
                baseURL = baseURL.Substring(0, baseURL.Length - 1);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("id_token=")
                    .Append(HttpUtility.UrlEncode(idToken))
                    .Append("&redirect_uri=")
                    .Append(HttpUtility.UrlEncode(redirectURI))
                    .Append("&logout=true");
            logoutURL = baseURL + "/v1/endsession?" + stringBuilder.ToString();
            return logoutURL;
        }
    }
}
