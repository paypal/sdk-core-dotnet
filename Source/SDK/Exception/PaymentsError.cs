using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        public string field { get; set; }

        /// <summary>
        /// Gets or sets the reason for the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string issue { get; set; }
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
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the PayPal internal identifier used for correlation purposes.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string debug_id { get; set; }

        /// <summary>
        /// Gets or sets the message describing the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }

        /// <summary>
        /// Gets or sets the URI for detailed information related to this error for the developer.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string information_link { get; set; }

        /// <summary>
        /// Gets or sets additional details of the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentsErrorDetails> details { get; set; }
    }
}
