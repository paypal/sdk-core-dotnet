using System;
using System.Collections.Generic;
using System.Text;

namespace PayPal.OpenidConnect
{
    public class CreateFromRefreshTokenParameters
    {

        /// <summary>
        /// Scope used in query parameters
        /// </summary>
        private const string SCOPE = "scope";

        /// <summary>
        /// Grant Type used in query parameters
        /// </summary>
        private const string GRANTTYPE = "grant_type";

        /// <summary>
        /// Refresh Token used in query parameters
        /// </summary>
        private const string REFRESHTOKEN = "refresh_token";

        /// <summary>
        /// Backing map
        /// </summary>
        private Dictionary<string, string> containerMapValue;

        public CreateFromRefreshTokenParameters()
        {
            containerMapValue = new Dictionary<string, string>();
            containerMapValue.Add(GRANTTYPE, "refresh_token");
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
        /// Set the scope
        /// </summary>
        /// <param name="scope"></param>
        public void setScope(string scope)
        {
            ContainerMap.Add(SCOPE, scope);
        }
        
        /// <summary>
        /// Set the Grant Type
        /// </summary>
        /// <param name="grantType"></param>
        public void setGrantType(string grantType)
        {
            ContainerMap.Add(GRANTTYPE, grantType);
        }

        /// <summary>
        /// Set the Refresh Token
        /// </summary>
        /// <param name="refreshToken"></param>
        public void setRefreshToken(string refreshToken)
        {
            ContainerMap.Add(REFRESHTOKEN, refreshToken);
        }
    }

}
