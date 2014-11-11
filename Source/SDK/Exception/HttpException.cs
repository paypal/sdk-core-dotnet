using System.Net;
using System;

namespace PayPal.Exception
{
    /// <summary>
    /// Represents an error occurred when attempting to send an HTTP request.
    /// </summary>
    public class HttpException : ConnectionException
    {
        /// <summary>
        /// Gets the <see cref="System.Net.HttpStatusCode"/> returned from the server.
        /// </summary>
        public HttpStatusCode StatusCode { get { return this._statusCode; } private set { this._statusCode = value; } }
        private HttpStatusCode _statusCode;

        /// <summary>
        /// Represents an error occurred when attempting to send an HTTP request.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="response">The response from server.</param>
        /// <param name="status">HTTP status code returned from the server.</param>
        public HttpException(string message, string response, HttpStatusCode statusCode, WebExceptionStatus webExceptionStatus) : base(message, response, webExceptionStatus)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Copy constructor provided by convenience for derived classes.
        /// </summary>
        /// <param name="ex">The original exception to copy information from.</param>
        protected HttpException(HttpException ex) : base(ex)
        {
            this.StatusCode = ex.StatusCode;
        }

        /// <summary>
        /// Attempts to convert this exception object to another specified exception type.
        /// </summary>
        /// <typeparam name="T">Object type that must derive from HttpException.</typeparam>
        /// <param name="other">Variable that will contain the newly created instance of the derviced class.</param>
        /// <returns>True if the object was successfully created; false otherwise.</returns>
        public bool TryConvertTo<T>(out T other) where T: HttpException
        {
            other = null;
            try
            {
                other = (T)Activator.CreateInstance(typeof(T), new object[] { this });
            }
            catch (System.Exception) { /* Silently ignore any error in converting. */}
            return other != null;
        }

        /// <summary>
        /// Gets the prefix to use when logging the exception information.
        /// </summary>
        protected override string ExceptionMessagePrefix { get { return "HTTP Exception"; } }

        /// <summary>
        /// Override of the default log message.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        protected override void LogDefaultMessage(string message) { }
    }
}
