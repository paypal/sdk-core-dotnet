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

#if NET_2_0
        /// <summary>
        /// OAuth Exception Short Message
        /// </summary>
        private string OAuthExcptnMessage;

        /// <summary>
        /// Gets and sets OAuth Exception Short Message
        /// </summary>
        public string OAuthExceptionMessage
        {
            get
            {
                return this.OAuthExcptnMessage;

            }
            set
            {
                this.OAuthExcptnMessage = value;
            }
        }

        /// <summary>
        /// OAuth Exception Long Message
        /// </summary>
        private string OAuthExcptnLongMessage;

        /// <summary>
        /// Gets and sets OAuth Exception Long Message
        /// </summary>
        public string OAuthExceptionLongMessage
        {
            get
            {
                return this.OAuthExcptnLongMessage;

            }
            set
            {
                this.OAuthExcptnLongMessage = value;
            }
        }
#else
        /// <summary>
        /// Gets and sets OAuth Exception Short Message
        /// </summary>
        public string OAuthExceptionMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets OAuth Exception Long Message
        /// </summary>
        public string OAuthExceptionLongMessage
        {
            get;
            set;
        }
#endif
        #endregion
    }
}
