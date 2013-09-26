using System.Collections.Generic;

namespace PayPal.OpenIdConnect
{
    public class CreateFromRefreshTokenParameters : ClientCredentials
    {
        /// <summary>
        /// Scope used in query parameters
        /// </summary>
        private const string Scope = "scope";

        /// <summary>
        /// Grant Type used in query parameters
        /// </summary>
        private const string GrantType = "grant_type";

        /// <summary>
        /// Refresh Token used in query parameters
        /// </summary>
        private const string RefreshToken = "refresh_token";

        /// <summary>
        /// Backing map
        /// </summary>
        private Dictionary<string, string> mapContainer;

        public CreateFromRefreshTokenParameters()
        {
            ContainerMap = new Dictionary<string, string>();
            ContainerMap.Add(GrantType, "refresh_token");
        }

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

        /// <summary>
        /// Set the scope
        /// </summary>
        /// <param name="scope"></param>
        public void SetScope(string scope)
        {
            ContainerMap.Add(Scope, scope);
        }

        /// <summary>
        /// Set the Grant Type
        /// </summary>
        /// <param name="grantType"></param>
        public void SetGrantType(string grantType)
        {
            ContainerMap.Add(GrantType, grantType);
        }

        /// <summary>
        /// Set the Refresh Token
        /// </summary>
        /// <param name="refreshToken"></param>
        public void SetRefreshToken(string refreshToken)
        {
            ContainerMap.Add(RefreshToken, refreshToken);
        }
    }
}
