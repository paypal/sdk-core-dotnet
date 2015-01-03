/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from "net20" for Visual Studio 2005 or "net35" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
using PayPal.Util;

namespace PayPal.OpenIdConnect
{
    public class Userinfo
    {
        /// <summary>
        /// Subject - Identifier for the End-User at the Issuer
        /// </summary>
        private string userId;

        /// <summary>
        /// Subject - Identifier for the End-User at the Issuer
        /// </summary>
        private string subject;

        /// <summary>
        /// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences
        /// </summary>
        private string fullName;

        /// <summary>
        /// Given name(s) or first name(s) of the End-User
        /// </summary>
        private string givenName;

        /// <summary>
        /// Surname(s) or last name(s) of the End-User
        /// </summary>
        private string familyName;

        /// <summary>
        /// Middle name(s) of the End-User
        /// </summary>
        private string middleName;

        /// <summary>
        /// URL of the End-User's profile picture
        /// </summary>
        private string profilePicture;

        /// <summary>
        /// End-User's preferred e-mail address
        /// </summary>
        private string emailAddress;

        /// <summary>
        /// True if the End-User's e-mail address has been verified; otherwise false
        /// </summary>
        private bool emailVerified;

        /// <summary>
        /// End-User's gender.
        /// </summary>
        private string userGender;

        /// <summary>
        /// End-User's birthday, represented as an YYYY-MM-DD format. They year MAY be 0000, indicating it is omited. To represent only the year, YYYY format would be used
        /// </summary>
        private string dateOfBirth;

        /// <summary>
        /// Time zone database representing the End-User's time zone
        /// </summary>
        private string zoneInformation;

        /// <summary>
        /// End-User's locale.
        /// </summary>
        private string localeName;

        /// <summary>
        /// End-User's preferred telephone number
        /// </summary>
        private string phoneNumber;

        /// <summary>
        /// End-User's preferred address
        /// </summary>
        private Address userAddress;
        /// <summary>
        /// Verified account status
        /// </summary>
        private bool verifiedAccount;

        /// <summary>
        /// Account type.
        /// </summary>
        private string accountType;

        /// <summary>
        /// Account holder age range.
        /// </summary>
        private string ageRange;

        /// <summary>
        /// Account payer identifier.
        /// </summary>
        private string payerId;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string user_id
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sub
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string given_name
        {
            get
            {
                return givenName;
            }
            set
            {
                givenName = value;
            }
        }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string family_name
        {
            get
            {
                return familyName;
            }
            set
            {
                familyName = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string middle_name
        {
            get
            {
                return middleName;
            }
            set
            {
                middleName = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string picture
        {
            get
            {
                return profilePicture;
            }
            set
            {
                profilePicture = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string email
        {
            get
            {
                return emailAddress;
            }
            set
            {
                emailAddress = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool email_verified
        {
            get
            {
                return emailVerified;
            }
            set
            {
                emailVerified = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string gender
        {
            get
            {
                return userGender;
            }
            set
            {
                userGender = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string birthdate
        {
            get
            {
                return dateOfBirth;
            }
            set
            {
                dateOfBirth = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string zoneinfo
        {
            get
            {
                return zoneInformation;
            }
            set
            {
                zoneInformation = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string locale
        {
            get
            {
                return localeName;
            }
            set
            {
                localeName = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string phone_number
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                phoneNumber = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Address address
        {
            get
            {
                return userAddress;
            }
            set
            {
                userAddress = value;
            }
        }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool verified_account
        {
            get
            {
                return verifiedAccount;
            }
            set
            {
                verifiedAccount = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string account_type
        {
            get
            {
                return accountType;
            }
            set
            {
                accountType = value;
            }
        }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string age_range
        {
            get
            {
                return ageRange;
            }
            set
            {
                ageRange = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string payer_id
        {
            get
            {
                return payerId;
            }
            set
            {
                payerId = value;
            }
        }

        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public Userinfo() { }

        /// <summary>
        /// Returns user details
        /// <param name="userinfoParameters">Query parameters used for API call</param>
        /// </summary>
        public static Userinfo GetUserinfo(UserinfoParameters userinfoParameters)
        {
            return GetUserinfo(null, userinfoParameters);
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
            string payLoad = "";
            if (apiContext == null)
            {
                apiContext = new APIContext();
            }
            apiContext.MaskRequestId = true;
            return PayPalResource.ConfigureAndExecute<Userinfo>(apiContext, HttpMethod.GET, resourcePath, payLoad);
        }
    }
}


