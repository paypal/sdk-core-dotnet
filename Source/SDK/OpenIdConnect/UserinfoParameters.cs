using System.Collections.Generic;
using System.Web;

namespace PayPal.OpenIdConnect
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

        /// <summary>
        /// Backing map
        /// </summary>
        private Dictionary<string, string> mapContainer;

        public UserinfoParameters()
        {
            ContainerMap = new Dictionary<string, string>();
            ContainerMap.Add(Schema, "openid");
        }

        /// <summary>
        /// Gets and sets the backing map
        /// </summary>
        public Dictionary<string, string> ContainerMap
        {
            get
            {
                return this.mapContainer;
            }
            set
            {
                this.mapContainer = value;
            }
        }
        /// <summary>
        /// Set the Access Token
        /// </summary>
        /// <param name="accessToken"></param>
        public void SetAccessToken(string accessToken)
        {
            ContainerMap.Add(AccessToken, HttpUtility.UrlEncode(accessToken));
        }
    }
}
