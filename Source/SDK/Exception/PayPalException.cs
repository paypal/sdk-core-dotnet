using PayPal.Log;

namespace PayPal.Exception
{
    public class PayPalException : System.Exception
    {
        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static Log4netLogger logger = Log4netLogger.GetLog4netLogger(typeof(PayPalException));

        /// <summary>
		/// Represents application configuration errors 
		/// </summary>
        public PayPalException() : base() { }
        
		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public PayPalException(string message): base(message)
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
        public PayPalException(string message, System.Exception cause) : base(message, cause)
		{
			//if (logger.IsErrorEnabled) 
			{
                logger.Error(this, message);
			}
		}
    }
}
