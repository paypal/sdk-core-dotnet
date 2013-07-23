using PayPal.Log;

namespace PayPal.Exception
{
    public class ConfigException : System.Exception
    {
        /// <summary>
        /// Logger
        /// </summary>
        //private static ILog logger = LogManagerWrapper.GetLogger(typeof(ConfigException));
        private static Log4netWrapper logger = Log4netWrapper.GetLogger(typeof(ConfigException));

		/// <summary>
		/// Represents application configuration errors 
		/// </summary>
        public ConfigException() : base() { }
        
		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public ConfigException(string message): base(message)
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
		public ConfigException(string message, System.Exception cause): base(message, cause)
		{
			//if (logger.IsErrorEnabled) 
			{
                logger.Error(this, message);
			}
		}
	}
}
