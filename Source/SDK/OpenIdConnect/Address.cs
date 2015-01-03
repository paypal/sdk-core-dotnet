/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from "net20" for Visual Studio 2005 or "net35" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;

namespace PayPal.OpenIdConnect
{
	public class Address
    {
        /// <summary>
        /// Street address component, which may include house number, and street name
        /// </summary>
        private string street;

        /// <summary>
        /// City or locality component
        /// </summary>
        private string localityName;

        /// <summary>
        /// State, province, prefecture or region component
        /// </summary>
        private string regionName;

        /// <summary>
        /// Zip code or postal code component
        /// </summary>
        private string post;

        /// <summary>
        /// Country name component.
        /// </summary>
        private string countryName;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string street_address
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
            }
        }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string locality
        {
            get
            {
                return localityName;
            }
            set
            {
                localityName = value;
            }
        }
   
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string region
        {
            get
            {
                return regionName;
            }
            set
            {
                regionName = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string postal_code
        {
            get
            {
                return post;
            }
            set
            {
                post = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string country
        {
            get
            {
                return countryName;
            }
            set
            {
                countryName = value;
            }
        }

        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public Address() { }
    }
}


