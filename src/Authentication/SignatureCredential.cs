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
        private string usrName;
                
        /// <summary>
        /// Password credential
        /// </summary>
        private string pasWord;

        /// <summary>
        /// Application ID (Used by Platform APIs)
        /// </summary>
        private string appID;

        /// <summary>
        /// Signature
        /// </summary>
        private string sign;

        /// <summary>
        /// Instance of IThirdPartyAuthorization
        /// </summary>
        private IThirdPartyAuthorization thrdPartyAuthorization;

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
            this.usrName = usrName;
            this.pasWord = pssword;
            this.sign = sign;
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

            this.thrdPartyAuthorization = thrdPartyAuthorization;
        }
        
        /// <summary>
        /// Gets and sets the Application ID
        /// </summary>
        public string ApplicationID
        {
            get
            {
                return appID;
            }

            set
            {
                this.appID = value;
            }
        }
       
        /// <summary>
        /// Gets and sets any instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get
            {
                return thrdPartyAuthorization;
            }
            set
            {
                this.thrdPartyAuthorization = value;
            }
        }

        /// <summary>
        /// Gets the UserName
        /// </summary>
        public string UserName
        {
            get
            {
                return usrName;
            }
        }

        /// <summary>
        /// Gets the Password
        /// </summary>
        public string Password
        {
            get
            {
                return pasWord;
            }
        }
        
        /// <summary>
        /// Gets the Signature
        /// </summary>
        public string Signature
        {
            get
            {
                return sign;
            }
        }
    }
}
