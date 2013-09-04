using System;

namespace PayPal.Authentication
{
    public class TokenAuthorization : IThirdPartyAuthorization
    {
        /// <summary>
        /// Access Token
        /// </summary>
        private string token;

        /// <summary>
        /// Access Token Secret
        /// </summary>
        private string tokenSecret;

        /// <summary>
        /// Token Authorization
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenSecret"></param>
        public TokenAuthorization(string token, string tokenSecret) : base()
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(tokenSecret))
            {
                throw new ArgumentException("TokenAuthorization arguments cannot be empty");
            }
            this.token = token;
            this.tokenSecret = tokenSecret;
        }
        
        /// <summary>
        /// Gets the Access Token
        /// </summary>
        public string AccessToken
        {
            get
            {
                return token;
            }
        }

        /// <summary>
        /// Gets the Access Token Secret
        /// </summary>
        public string AccessTokenSecret
        {
            get
            {
                return tokenSecret;
            }
        }
    }
}
