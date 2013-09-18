using System.Collections.Generic;

namespace PayPal.OpenIdConnect
{
    public class CreateFromAuthorizationCodeParameters : ClientCredentials
    {        
        /// <summary>
        /// Code used in query parameters
        /// </summary>
        private const string Code = "code";

        /// <summary>
        /// Redirect URI used in query parameters
        /// </summary>
        private const string RedirectUri = "redirect_uri";

        /// <summary>
        /// Grant Type used in query parameters
        /// </summary>
        private const string GrantType = "grant_type";

        /// <summary>
        /// Backing map
        /// </summary>
        private Dictionary<string, string> mapContainer;

        /// <summary>
        /// Backing map
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

        public CreateFromAuthorizationCodeParameters()
        {
            ContainerMap = new Dictionary<string, string>();
            ContainerMap.Add(GrantType, "authorization_code");
        }        

        /// <summary>
        /// Set the code
        /// </summary>
        /// <param name="code"></param>
        public void SetCode(string code)
        {
            ContainerMap.Add(Code, code);
        }

        /// <summary>
        /// Set the Redirect URI
        /// </summary>
        /// <param name="redirectUri"></param>
        public void SetRedirectUri(string redirectUri)
        {
            ContainerMap.Add(RedirectUri, redirectUri);
        }

        /// <summary>
        /// Set the Grant Type
        /// </summary>
        /// <param name="grantType"></param>
        public void SetGrantType(string grantType)
        {
            ContainerMap.Add(GrantType, grantType);
        }
    }
}
