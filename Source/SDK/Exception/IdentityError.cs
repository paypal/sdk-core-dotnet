using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PayPal.Exception
{
    /// <summary>
    /// Represents a JSON error object returned from requests made to the Identity API.
    /// More information: https://developer.paypal.com/webapps/developer/docs/api/#common-identity-objects
    /// </summary>
    public class IdentityError
    {
        /// <summary>
        /// Gets or sets an ASCII error code. See above link for full list of potential error codes.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error { get { return this._error; } set { this._error = value; } }
        private string _error;

        /// <summary>
        /// Gets or sets the details concerning this error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error_description { get { return this._error_description; } set { this._error_description = value; } }
        private string _error_description;

        /// <summary>
        /// Gets or sets a URI identifying a human-readable web page with information about the error. This is used to provide the client
        /// developer with additional information about the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error_uri { get { return this._error_uri; } set { this._error_uri = value; } }
        private string _error_uri;
    }
}
