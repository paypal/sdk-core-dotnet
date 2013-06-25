using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace PayPal.OpenidConnect
{
    public class UserinfoParameters
    {
        /// <summary>
        /// Schema used in query parameters
        /// </summary>
        private const string Schema = "schema";

        /// <summary>
        /// Access Token used in query parameters
        /// </summary>
        private const string AccessToken = "access_token";

        public UserinfoParameters()
        {
            ContainerMap = new Dictionary<string, string>();
            ContainerMap.Add(Schema, "openid");
        }

        /// <summary>
        /// Backing map
        /// </summary>
        public Dictionary<string, string> ContainerMap
        {
            get;
            set;
        }

        /// <summary>
        /// Set the Access Token
        /// </summary>
        /// <param name="accessToken"></param>
        public void setAccessToken(string accessToken)
        {
            ContainerMap.Add(AccessToken, HttpUtility.UrlEncode(accessToken));
        }
    }

}
