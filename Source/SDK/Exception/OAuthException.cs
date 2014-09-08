namespace PayPal.Exception
{
    public class OAuthException : PayPalException
    {
        /// <summary>
        /// Represents an exception related to requesting an OAuth token.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        public OAuthException(string message) : this(message, null) { }

        /// <summary>
        /// Represents an exception related to requesting an OAuth token.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        /// <param name="exception">More exception information that should be included with this exception.</param>
        public OAuthException(string message, System.Exception exception) : base(message, exception) { }

        /// <summary>
        /// Gets the OAuth exception short message
        /// </summary>
        public string OAuthExceptionShortMessage { get { return this.Message; } }

        /// <summary>
        /// Gets the OAuth exception long message
        /// </summary>
        public string OAuthExceptionLongMessage { get { return this.InnerException == null ? string.Empty : this.InnerException.Message; } }

        /// <summary>
        /// Gets the prefix to use when logging the exception information.
        /// </summary>
        protected override string ExceptionMessagePrefix { get { return "OAuth Exception"; } }
    }
}
