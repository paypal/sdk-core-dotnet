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

namespace PayPal.OpenidConnect
{
	public class Address
	{
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
		
	}
}


