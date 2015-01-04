using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayPal.Exception
{
    /// <summary>
    /// Represents more detailed information about a specific Payments error.
    /// </summary>
    public class PaymentsErrorDetails
    {
        /// <summary>
        /// Gets or sets the name of the field that caused the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string field { get { return this._field; } set { this._field = value; } }
        private string _field;

        /// <summary>
        /// Gets or sets the reason for the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string issue { get { return this._issue; } set { this._issue = value; } }
        private string _issue;
    }

    /// <summary>
    /// Represents an error returned from the PayPal Payments API.
    /// More information: https://developer.paypal.com/webapps/developer/docs/api/#common-payments-objects
    /// See the section "error object (for Payments)"
    /// </summary>
    public class PaymentsError
    {
        /// <summary>
        /// Gets or sets the human readable, unique name of the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get { return this._name; } set { this._name = value; } }
        private string _name;

        /// <summary>
        /// Gets or sets the PayPal internal identifier used for correlation purposes.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string debug_id { get { return this._debug_id; } set { this._debug_id = value; } }
        private string _debug_id;

        /// <summary>
        /// Gets or sets the message describing the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get { return this._message; } set { this._message = value; } }
        private string _message;

        /// <summary>
        /// Gets or sets the URI for detailed information related to this error for the developer.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string information_link { get { return this._information_link; } set { this._information_link = value; } }
        private string _information_link;

        /// <summary>
        /// Gets or sets additional details of the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentsErrorDetails> details { get { return this._details; } set { this._details = value; } }
        private List<PaymentsErrorDetails> _details;
    }
}
