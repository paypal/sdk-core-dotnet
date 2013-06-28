/* NuGet Install
 * Visual Studio 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from the folder "net35"
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PayPal.OpenIDConnect
{
	public class Address
    {
#if NET_2_0
        /// <summary>
        /// Full street address component, which may include house number, street name.
        /// </summary>
        private string street_addressValue;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string street_address
        {
            get
            {
                return street_addressValue;
            }
            set
            {
                street_addressValue = value;
            }
        }
        /// <summary>
        /// City or locality component.
        /// </summary>
        private string localityValue;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string locality
        {
            get
            {
                return localityValue;
            }
            set
            {
                localityValue = value;
            }
        }
        /// <summary>
        /// State, province, prefecture or region component.
        /// </summary>
        private string regionValue;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string region
        {
            get
            {
                return regionValue;
            }
            set
            {
                regionValue = value;
            }
        }
        /// <summary>
        /// Zip code or postal code component.
        /// </summary>
        private string postal_codeValue;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string postal_code
        {
            get
            {
                return postal_codeValue;
            }
            set
            {
                postal_codeValue = value;
            }
        }
        /// <summary>
        /// Country name component.
        /// </summary>
        private string countryValue;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string country
        {
            get
            {
                return countryValue;
            }
            set
            {
                countryValue = value;
            }
        }
#else
        /// <summary>
        /// Full street address component, which may include house number, street name
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string street_address
        {
            get;
            set;
        }

        /// <summary>
        /// City or locality component
        /// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string locality
		{
            get;
            set;
		}

		/// <summary>
        /// State, province, prefecture or region component
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string region
		{
            get;
            set;
		}
		
        /// <summary>
        /// Zip code or postal code component
        /// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string postal_code
		{
            get;
            set;
		}
		
        /// <summary>
        /// Country name component
        /// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string country
		{
            get;
            set;
		}	
#endif
        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public Address() { }
    }
}


