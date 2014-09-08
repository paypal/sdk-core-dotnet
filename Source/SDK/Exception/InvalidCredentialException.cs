namespace PayPal.Exception
{
    public class InvalidCredentialException : PayPalException
    {
        /// <summary>
        /// Represents errors where certain credential information is invalid.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public InvalidCredentialException(string message) : base(message) { }

        /// <summary>
        /// Gets the prefix to use when logging the exception information.
        /// </summary>
        protected override string ExceptionMessagePrefix { get { return "Invalid Credential"; } }
    }
}
