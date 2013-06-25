namespace PayPal.Exception
{
    public class OAuthException : System.Exception
    {
        #region Priavte Members
        /// <summary>
        /// Short message
        /// </summary>
        private string oauthExpMessage;
        /// <summary>
        /// Long message
        /// </summary>
        private string oauthExpLongMessage;

        #endregion

        #region Constructors

        public OAuthException(string oauthExceptionMessage, System.Exception exception)
        {
            this.oauthExpMessage = oauthExceptionMessage;
            this.oauthExpLongMessage = exception.Message;
        }
        public OAuthException(string oauthExceptionMessage)
        {
            this.oauthExpMessage = oauthExceptionMessage;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Short message.
        /// </summary>
        public string OauthExceptionMessage
        {
            get
            {
                return oauthExpMessage;
            }
            set
            {
                oauthExpMessage = value;
            }
        }

        /// <summary>
        /// Long message
        /// </summary>
        public string OauthExceptionLongMessage
        {
            get
            {
                return oauthExpLongMessage;
            }
            set
            {
                oauthExpLongMessage = value;
            }
        }

        #endregion       
    }
}
