using PayPal.Log;

namespace PayPal.Exception
{
    public class MissingCredentialException : System.Exception
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(MissingCredentialException));

		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		public MissingCredentialException() : base() {}
        
		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public MissingCredentialException(string message): base(message)
		{
            logger.Error(message, this);
        }
        
		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
        public MissingCredentialException(string message, System.Exception innerException): base(message, innerException)
        {
            logger.Error(message, this);
        }
	}
}
