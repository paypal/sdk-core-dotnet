namespace PayPal.Exception
{
    public class OAuthException : System.Exception
    {
        /// <summary>
        /// OAuth Exception Short Message
        /// </summary>
        private string oauthExcptnMessage;

        /// <summary>
        /// OAuth Exception Long Message
        /// </summary>
        private string oauthExcptnLongMessage;

        public OAuthException(string oauthExceptionMessage, System.Exception exception)
        {
            this.OAuthExceptionMessage = oauthExceptionMessage;
            this.OAuthExceptionLongMessage = exception.Message;
        }

        public OAuthException(string oauthExceptionMessage)
        {
            this.OAuthExceptionMessage = oauthExceptionMessage;
        }

        /// <summary>
        /// Gets and sets OAuth Exception Short Message
        /// </summary>
        public string OAuthExceptionMessage
        {
            get
            {
                return this.oauthExcptnMessage;

            }
            set
            {
                this.oauthExcptnMessage = value;
            }
        }

        /// <summary>
        /// Gets and sets OAuth Exception Long Message
        /// </summary>
        public string OAuthExceptionLongMessage
        {
            get
            {
                return this.oauthExcptnLongMessage;

            }
            set
            {
                this.oauthExcptnLongMessage = value;
            }
        }
    }
}
