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
        /// <summary>
        /// Full street address component, which may include house number, street name.
        /// </summary>
        private string street_addressValue;

        /// <summary>
        /// City or locality component.
        /// </summary>
        private string localityValue;

        /// <summary>
        /// State, province, prefecture or region component.
        /// </summary>
        private string regionValue;

        /// <summary>
        /// Zip code or postal code component.
        /// </summary>
        private string postal_codeValue;

        /// <summary>
        /// Country name component.
        /// </summary>
        private string countryValue;

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

        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public Address() { }
    }
}


