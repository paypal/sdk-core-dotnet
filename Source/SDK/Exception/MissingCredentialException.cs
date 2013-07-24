using PayPal.Log;

namespace PayPal.Exception
{
    public class MissingCredentialException : System.Exception
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static Log4netLogger logger = Log4netLogger.GetLogger(typeof(MissingCredentialException));

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
			//if (logger.IsErrorEnabled)
			{
				logger.Error(this, message);
			}
		}
        
		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		/// <param name="cause">The exception that is the cause of the current exception</param>
        public MissingCredentialException(string message, System.Exception cause)
            : base(message, cause)
		{
			//if (logger.IsErrorEnabled) 
			{
                logger.Error(this, message);
			}
		}
	}
}
