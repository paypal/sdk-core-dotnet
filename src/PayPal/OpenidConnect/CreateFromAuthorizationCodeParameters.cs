using System;
using System.Collections.Generic;
using System.Text;

namespace PayPal.OpenidConnect
{
    public class CreateFromAuthorizationCodeParameters
    {
        
        /// <summary>
        /// Code used in query parameters
        /// </summary>
        private const string CODE = "code";

        /// <summary>
        /// Redirect URI used in query parameters
        /// </summary>
        private const string REDIRECTURI = "redirect_uri";

        /// <summary>
        /// Grant Type used in query parameters
        /// </summary>
        private const string GRANTTYPE = "grant_type";

        /// <summary>
        /// Backing map
        /// </summary>
        private Dictionary<string, string> containerMapValue;

        public CreateFromAuthorizationCodeParameters()
        {
            containerMapValue = new Dictionary<string, string>();
            containerMapValue.Add(GRANTTYPE, "authorization_code");
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
        /// Set the code
        /// </summary>
        /// <param name="code"></param>
        public void setCode(string code)
        {
            ContainerMap.Add(CODE, code);
        }

        /// <summary>
        /// Set the Redirect URI
        /// </summary>
        /// <param name="redirectURI"></param>
        public void setRedirectURI(string redirectURI)
        {
            ContainerMap.Add(REDIRECTURI, redirectURI);
        }

        /// <summary>
        /// Set the Grant Type
        /// </summary>
        /// <param name="grantType"></param>
        public void setGrantType(string grantType)
        {
            ContainerMap.Add(GRANTTYPE, grantType);
        }
    }
}
