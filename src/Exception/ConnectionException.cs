namespace PayPal.Exception
{
    public class ConnectionException : System.Exception
    {
        private string response;

        /// <summary>
        /// Response payload for non-200 response
        /// </summary>
        public string Response
        {
            get
            {
                return this.response;
            }
            private set {
                this.response = value;
            }
        }

        public ConnectionException() : base() { }

		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
        public ConnectionException(string message) : base(message) { }

        /// <summary>
        /// Represents errors that occur during application execution
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="response">The response from server</param>
        public ConnectionException(string message, string response) : base(message)
        {
            this.response = response;
        }
    }
}
