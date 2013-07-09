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
        private string userNameCredential;
                
        /// <summary>
        /// Password credential
        /// </summary>
        private string passwordCredential;

        /// <summary>
        /// Signature
        /// </summary>
        private string sign;

        /// <summary>
        /// Third Party Authorization
        /// </summary>
        private IThirdPartyAuthorization authorization;

        /// <summary>
        ///  Application Id
        /// </summary>
        private string appId;

        /// <summary>
        /// SignatureCredential constructor
        /// </summary>
        /// <param name="userNameCredential"></param>
        /// <param name="passwordCredential"></param>
        /// <param name="sign"></param>
        public SignatureCredential(string userNameCredential, string passwordCredential, string sign) 
            : base()
        {
            if (string.IsNullOrEmpty(userNameCredential) || string.IsNullOrEmpty(passwordCredential) ||
                string.IsNullOrEmpty(sign))
            {
                throw new ArgumentException("Signature Credential arguments cannot be null");
            }
            this.userNameCredential = userNameCredential;
            this.passwordCredential = passwordCredential;
            this.sign = sign;
        }

        /// <summary>
        /// SignatureCredential constructor overload
        /// </summary>
        /// <param name="userNameCredential"></param>
        /// <param name="passwordCredential"></param>
        /// <param name="sign"></param>
        /// <param name="thrdPartyAuthorization"></param>     
        public SignatureCredential(string userNameCredential, string passwordCredential, string sign, 
            IThirdPartyAuthorization thrdPartyAuthorization)
            : this(userNameCredential, passwordCredential, sign)
        {
            this.ThirdPartyAuthorization = thrdPartyAuthorization;
        }
        
        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get
            {
                return this.authorization;
            }
            set
            {
                this.authorization = value;
            }
        }

        /// <summary>
        /// Gets and sets the Application Id (Used by Platform APIs)
        /// </summary>
        public string ApplicationId
        {
            get
            {
                return this.appId;
            }
            set
            {
                this.appId = value;
            }
        }

        /// <summary>
        /// Gets the UserName
        /// </summary>
        public string UserName
        {
            get
            {
                return userNameCredential;
            }
        }

        /// <summary>
        /// Gets the Password
        /// </summary>
        public string Password
        {
            get
            {
                return passwordCredential;
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
