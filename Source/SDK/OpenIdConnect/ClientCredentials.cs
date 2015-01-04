namespace PayPal.OpenIdConnect
{
    public abstract class ClientCredentials
    {
        // Client ID 
        public string clientId;

        // Client Secret
        public string clientSecret;

        /// <summary>
        /// Set the Client ID
        /// </summary>
        /// <param name="clientId"></param>
        public void setClientId(string clientId)
        {
            this.clientId = clientId;
        }

        /// <summary>
        /// Set the Client Secret
        /// </summary>
        /// <param name="clientSecret"></param>
        public void setClientSecret(string clientSecret)
        {
            this.clientSecret = clientSecret;
        }

        /// <summary>
        /// Returns the Client ID
        /// </summary>
        /// <returns></returns>
        public string getClientId()
        {
            return this.clientId;
        }

        /// <summary>
        /// Returns the Client Secret
        /// </summary>
        /// <returns></returns>
        public string getClientSecret()
        {
            return this.clientSecret;
        }

    }
}
