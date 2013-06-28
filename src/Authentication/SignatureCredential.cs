using System;

namespace PayPal.Authentication
{
    /// <summary>
    /// SignatureCredential 
    /// Encapsulates signature credential information 
    /// used by service authentication systems
    /// </summary>
    public class SignatureCredential : ICredential
    {
        /// <summary>
        /// Username credential
        /// </summary> 
        private string UsrName;
                
        /// <summary>
        /// Password credential
        /// </summary>
        private string PssWord;

        /// <summary>
        /// Signature
        /// </summary>
        private string Sign;

        /// <summary>
        /// SignatureCredential constructor
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pssword"></param>
        /// <param name="sign"></param>
        public SignatureCredential(string usrName, string pssword, string sign) 
            : base()
        {
            if (string.IsNullOrEmpty(usrName) || string.IsNullOrEmpty(pssword) ||
                string.IsNullOrEmpty(sign))
            {
                throw new ArgumentException("Signature Credential arguments cannot be null");
            }
            this.UsrName = usrName;
            this.PssWord = pssword;
            this.Sign = sign;
        }

        /// <summary>
        /// SignatureCredential constructor overload
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pasWord"></param>
        /// <param name="sign"></param>
        /// <param name="thrdPartyAuthorization"></param>
     
        public SignatureCredential(string usrName, string pasWord, string sign, 
            IThirdPartyAuthorization thrdPartyAuthorization)
            : this(usrName, pasWord, sign)
        {

#if NET_2_0
            this.Authorization = thrdPartyAuthorization;
#else
            this.ThirdPartyAuthorization = thrdPartyAuthorization;
#endif
        }

#if NET_2_0
        /// <summary>
        /// Third Party Authorization
        /// </summary>
        private IThirdPartyAuthorization Authorization;

        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get
            {
                return this.Authorization;
            }
            set
            {
                this.Authorization = value;
            }
        }

        /// <summary>
        ///  Application ID
        /// </summary>
        private string AppID;

        /// <summary>
        /// Gets and sets the Application ID (Used by Platform APIs)
        /// </summary>
        public string ApplicationID
        {
            get
            {
                return this.AppID;
            }
            set
            {
                this.AppID = value;
            }
        }
#else
        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the Application ID (Used by Platform APIs)
        /// </summary>
        public string ApplicationID
        {
            get;
            set;
        }
#endif

        /// <summary>
        /// Gets the UserName
        /// </summary>
        public string UserName
        {
            get
            {
                return UsrName;
            }
        }

        /// <summary>
        /// Gets the Password
        /// </summary>
        public string Password
        {
            get
            {
                return PssWord;
            }
        }
        
        /// <summary>
        /// Gets the Signature
        /// </summary>
        public string Signature
        {
            get
            {
                return Sign;
            }
        }
    }
}
