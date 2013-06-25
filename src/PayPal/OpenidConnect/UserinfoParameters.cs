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
        private const string SCHEMA = "schema";

        /// <summary>
        /// Access Token used in query parameters
        /// </summary>
        private const string ACCESSTOKEN = "access_token";

        /// <summary>
        /// Backing map
        /// </summary>
        private Dictionary<string, string> containerMapValue;

        public UserinfoParameters()
        {
            containerMapValue = new Dictionary<string, string>();
            containerMapValue.Add(SCHEMA, "openid");
        }

        public Dictionary<string, string> ContainerMap
        {
            get
            {
                return containerMapValue;
            }
            set
            {
                containerMapValue = value;
            }
        }

        /// <summary>
        /// Set the Access Token
        /// </summary>
        /// <param name="accessToken"></param>
        public void setAccessToken(string accessToken)
        {
            ContainerMap.Add(ACCESSTOKEN, HttpUtility.UrlEncode(accessToken));
        }
    }

}
