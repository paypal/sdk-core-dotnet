namespace PayPal.Exception
{
    public class ConfigException : PayPalException
    {
		/// <summary>
		/// Represents errors that are related to the application's configuration.
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public ConfigException(string message): base(message) { }

        /// <summary>
        /// Gets the prefix to use when logging the exception information.
        /// </summary>
        protected override string ExceptionMessagePrefix { get { return "Configuration Exception"; } }
	}
}
