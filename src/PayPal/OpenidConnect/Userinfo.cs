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
		/// Subject - Identifier for the End-User at the Issuer.
		/// </summary>
		private string user_idValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string user_id
		{
			get
			{
				return user_idValue;
			}
			set
			{
				user_idValue = value;
			}
		}
		/// <summary>
		/// Subject - Identifier for the End-User at the Issuer.
		/// </summary>
		private string subValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string sub
		{
			get
			{
				return subValue;
			}
			set
			{
				subValue = value;
			}
		}
		/// <summary>
		/// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences.
		/// </summary>
		private string nameValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string name
		{
			get
			{
				return nameValue;
			}
			set
			{
				nameValue = value;
			}
		}
		/// <summary>
		/// Given name(s) or first name(s) of the End-User
		/// </summary>
		private string given_nameValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string given_name
		{
			get
			{
				return given_nameValue;
			}
			set
			{
				given_nameValue = value;
			}
		}
		/// <summary>
		/// Surname(s) or last name(s) of the End-User.
		/// </summary>
		private string family_nameValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string family_name
		{
			get
			{
				return family_nameValue;
			}
			set
			{
				family_nameValue = value;
			}
		}
		/// <summary>
		/// Middle name(s) of the End-User.
		/// </summary>
		private string middle_nameValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string middle_name
		{
			get
			{
				return middle_nameValue;
			}
			set
			{
				middle_nameValue = value;
			}
		}
		/// <summary>
		/// URL of the End-User's profile picture.
		/// </summary>
		private string pictureValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string picture
		{
			get
			{
				return pictureValue;
			}
			set
			{
				pictureValue = value;
			}
		}
		/// <summary>
		/// End-User's preferred e-mail address.
		/// </summary>
		private string emailValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string email
		{
			get
			{
				return emailValue;
			}
			set
			{
				emailValue = value;
			}
		}
		/// <summary>
		/// True if the End-User's e-mail address has been verified; otherwise false.
		/// </summary>
		private bool email_verifiedValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool email_verified
		{
			get
			{
				return email_verifiedValue;
			}
			set
			{
				email_verifiedValue = value;
			}
		}
		/// <summary>
		/// End-User's gender.
		/// </summary>
		private string genderValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string gender
		{
			get
			{
				return genderValue;
			}
			set
			{
				genderValue = value;
			}
		}
		/// <summary>
		/// End-User's birthday, represented as an YYYY-MM-DD format. They year MAY be 0000, indicating it is omited. To represent only the year, YYYY format would be used.
		/// </summary>
		private string birthdateValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string birthdate
		{
			get
			{
				return birthdateValue;
			}
			set
			{
				birthdateValue = value;
			}
		}
		/// <summary>
		/// Time zone database representing the End-User's time zone
		/// </summary>
		private string zoneinfoValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string zoneinfo
		{
			get
			{
				return zoneinfoValue;
			}
			set
			{
				zoneinfoValue = value;
			}
		}
		/// <summary>
		/// End-User's locale.
		/// </summary>
		private string localeValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string locale
		{
			get
			{
				return localeValue;
			}
			set
			{
				localeValue = value;
			}
		}
		/// <summary>
		/// End-User's preferred telephone number.
		/// </summary>
		private string phone_numberValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string phone_number
		{
			get
			{
				return phone_numberValue;
			}
			set
			{
				phone_numberValue = value;
			}
		}
		/// <summary>
		/// End-User's preferred address.
		/// </summary>
		private Address addressValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Address address
		{
			get
			{
				return addressValue;
			}
			set
			{
				addressValue = value;
			}
		}
		/// <summary>
		/// Verified account status.
		/// </summary>
		private bool verified_accountValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool verified_account
		{
			get
			{
				return verified_accountValue;
			}
			set
			{
				verified_accountValue = value;
			}
		}
		/// <summary>
		/// Account type.
		/// </summary>
		private string account_typeValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string account_type
		{
			get
			{
				return account_typeValue;
			}
			set
			{
				account_typeValue = value;
			}
		}
		/// <summary>
		/// Account holder age range.
		/// </summary>
		private string age_rangeValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string age_range
		{
			get
			{
				return age_rangeValue;
			}
			set
			{
				age_rangeValue = value;
			}
		}
		/// <summary>
		/// Account payer identifier.
		/// </summary>
		private string payer_idValue;
	
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string payer_id
		{
			get
			{
				return payer_idValue;
			}
			set
			{
				payer_idValue = value;
			}
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


