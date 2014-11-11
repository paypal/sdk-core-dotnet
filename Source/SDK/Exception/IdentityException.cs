using System.Text;
using Newtonsoft.Json;

namespace PayPal.Exception
{
    /// <summary>
    /// Represents Identity API errors related to logging into PayPal securely using PayPal login credentials.
    /// More Information: https://developer.paypal.com/webapps/developer/docs/api/#identity
    /// </summary>
    public class IdentityException : HttpException
    {
        /// <summary>
        /// Gets a <see cref="PayPal.Exception.IdentityError"/> JSON object containing the parsed details of the Identity error.
        /// </summary>
        public IdentityError Details { get { return this._details; } private set { this._details = value; } }
        private IdentityError _details;

        /// <summary>
        /// Copy constructor that attempts to deserialize the response from the specified <paramref name="PayPal.Exception.HttpException"/>.
        /// </summary>
        /// <param name="ex">Originating <see cref="PayPal.Exception.HttpException"/> object that contains the details of the exception.</param>
        public IdentityException(HttpException ex) : base(ex)
        {
            if (!string.IsNullOrEmpty(this.Response))
            {
                this.Details = JsonConvert.DeserializeObject<IdentityError>(this.Response);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("   Error:   " + this.Details.error);
                sb.AppendLine("   Message: " + this.Details.error_description);
                sb.AppendLine("   URI:     " + this.Details.error_uri);

                this.LogMessage(sb.ToString());
            }
        }

        /// <summary>
        /// Gets the prefix to use when logging the exception information.
        /// </summary>
        protected override string ExceptionMessagePrefix { get { return "Identity Exception"; } }
    }
}
