namespace PayPal.Exception
{
    public class OAuthException : System.Exception
    {
        /// <summary>
        /// OAuth Exception Short Message
        /// </summary>
        private string oauthExceptionMessageShort;

        /// <summary>
        /// OAuth Exception Long Message
        /// </summary>
        private string oauthExceptionMessageLong;

        public OAuthException(string oauthExceptionMessage, System.Exception exception)
        {
            this.OAuthExceptionShortMessage = oauthExceptionMessage;
            this.OAuthExceptionLongMessage = exception.Message;
        }

        public OAuthException(string oauthExceptionMessage)
        {
            this.OAuthExceptionShortMessage = oauthExceptionMessage;
        }

        /// <summary>
        /// Gets and sets OAuth Exception Short Message
        /// </summary>
        public string OAuthExceptionShortMessage
        {
            get
            {
                return this.oauthExceptionMessageShort;

            }
            set
            {
                this.oauthExceptionMessageShort = value;
            }
        }

        /// <summary>
        /// Gets and sets OAuth Exception Long Message
        /// </summary>
        public string OAuthExceptionLongMessage
        {
            get
            {
                return this.oauthExceptionMessageLong;

            }
            set
            {
                this.oauthExceptionMessageLong = value;
            }
        }
    }
}
