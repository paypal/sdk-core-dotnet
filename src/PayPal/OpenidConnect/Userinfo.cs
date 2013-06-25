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
using PayPal.Util;

namespace PayPal.OpenidConnect
{
    public class Userinfo
    {
        /// <summary>
        /// Subject - Identifier for the End-User at the Issuer
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string user_id
        {
            get;
            set;
        }

        /// <summary>
        /// Subject - Identifier for the End-User at the Issuer
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sub
        {
            get;
            set;
        }

        /// <summary>
        /// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Given name(s) or first name(s) of the End-User
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string given_name
        {
            get;
            set;
        }

        /// <summary>
        /// Surname(s) or last name(s) of the End-User
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string family_name
        {
            get;
            set;
        }

        /// <summary>
        /// Middle name(s) of the End-User
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string middle_name
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the End-User's profile picture
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string picture
        {
            get;
            set;
        }

        /// <summary>
        /// End-User's preferred e-mail address
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string email
        {
            get;
            set;
        }

        /// <summary>
        /// True if the End-User's e-mail address has been verified; otherwise false
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool email_verified
        {
            get;
            set;
        }

        /// <summary>
        /// End-User's gender
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string gender
        {
            get;
            set;
        }

        /// <summary>
        /// End-User's birthday, represented as an YYYY-MM-DD format. They year MAY be 0000, indicating it is omited. To represent only the year, YYYY format would be used
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string birthdate
        {
            get;
            set;
        }

        /// <summary>
        /// Time zone database representing the End-User's time zone
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string zoneinfo
        {
            get;
            set;
        }

        /// <summary>
        /// End-User's locale
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string locale
        {
            get;
            set;
        }

        /// <summary>
        /// End-User's preferred telephone number
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string phone_number
        {
            get;
            set;
        }

        /// <summary>
        /// End-User's preferred address
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Address address
        {
            get;
            set;
        }

        /// <summary>
        /// Verified account status
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool verified_account
        {
            get;
            set;
        }

        /// <summary>
        /// Account type
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string account_type
        {
            get;
            set;
        }

        /// <summary>
        /// Account holder age range
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string age_range
        {
            get;
            set;
        }

        /// <summary>
        /// Account payer identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string payer_id
        {
            get;
            set;
        }

        /// <summary>
        /// Returns user details
        /// <param name="userinfoParameters">Query parameters used for API call</param>
        /// </summary>
        public static Userinfo GetUserinfo(UserinfoParameters userinfoParameters)
        {
            string pattern = "v1/identity/openidconnect/userinfo?schema={0}&access_token={1}";
            object[] parameters = new object[] { userinfoParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = string.Empty;
            return PayPalResource.ConfigureAndExecute<Userinfo>(null, HttpMethod.GET,
                    resourcePath, null, payLoad);
        }

        /// <summary>
        /// Returns user details
        /// <param name="apiContext">APIContext to be used for the call.</param>
        /// <param name="userinfoParameters">Query parameters used for API call</param>
        /// </summary>
        public static Userinfo GetUserinfo(APIContext apiContext, UserinfoParameters userinfoParameters)
        {
            string pattern = "v1/identity/openidconnect/userinfo?schema={0}&access_token={1}";
            object[] parameters = new object[] { userinfoParameters };
            string resourcePath = SDKUtil.FormatURIPath(pattern, parameters);
            string payLoad = string.Empty;
            return PayPalResource.ConfigureAndExecute<Userinfo>(apiContext,
                    HttpMethod.GET, resourcePath, null, payLoad);
        }
    }
}


