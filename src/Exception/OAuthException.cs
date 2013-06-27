namespace PayPal.Exception
{
    public class OAuthException : System.Exception
    {
        #region Constructors

        public OAuthException(string oauthExceptionMessage, System.Exception exception)
        {
            this.OAuthExceptionMessage = oauthExceptionMessage;
            this.OAuthExceptionLongMessage = exception.Message;
        }

        public OAuthException(string oauthExceptionMessage)
        {
            this.OAuthExceptionMessage = oauthExceptionMessage;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Short message.
        /// </summary>
        public string OAuthExceptionMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Long message
        /// </summary>
        public string OAuthExceptionLongMessage
        {
            get;
            set;
        }

        #endregion       
    }
}
