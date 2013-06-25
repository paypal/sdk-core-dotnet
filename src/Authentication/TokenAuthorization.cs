using System;

namespace PayPal.Authentication
{
    public class TokenAuthorization : IThirdPartyAuthorization
    {
        /// <summary>
        /// Access Token
        /// </summary>
        private string accssToken;

        /// <summary>
        /// Token Secret
        /// </summary>
        private string toknSecret;

        /// <summary>
        /// TokenAuthorization
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="tokenSecret"></param>
        public TokenAuthorization(string accssToken, string toknSecret) : base()
        {
            if (string.IsNullOrEmpty(accssToken) || string.IsNullOrEmpty(toknSecret))
            {
                throw new ArgumentException("TokenAuthorization arguments cannot be empty");
            }
            this.accssToken = accssToken;
            this.toknSecret = toknSecret;
        }
        
        /// <summary>
        /// Gets the Access Token
        /// </summary>
        public string AccessToken
        {
            get
            {
                return accssToken;
            }
        }

        /// <summary>
        /// Gets the Token Secret
        /// </summary>
        public string TokenSecret
        {
            get
            {
                return toknSecret;
            }
        }
    }
}
