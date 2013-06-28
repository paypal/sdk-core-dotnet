using System;

namespace PayPal.Authentication
{
    /// <summary>
    /// CertificateCredential
    /// Encapsulates certificate credential information
    /// used by service authentication systems
    /// </summary>
    public class CertificateCredential : ICredential
    {
        /// <summary>
        /// Username credential
        /// </summary>
        private string usrName;

        /// <summary>
        /// Password credential
        /// </summary>
        private string pssWord;              

        /// <summary>
        /// Certificate file
        /// </summary>
        private string certFile;

        /// <summary>
        /// Password of the Certificate's Private Key
        /// </summary>
        private string priKeyPassword;

        /// <summary>
        /// Third Party Authorization
        /// </summary>
        private IThirdPartyAuthorization authorization;

        /// <summary>
        ///  Application ID
        /// </summary>
        private string appID;
        /// <summary>
        /// CertificateCredential constructor
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pasWord"></param>
        /// <param name="certFile"></param>
        /// <param name="priKeyPassword"></param>
        public CertificateCredential(string usrName, string pasWord, string certFile, string priKeyPassword) 
            : base()
        {
            if (string.IsNullOrEmpty(usrName) || string.IsNullOrEmpty(pasWord) ||
                string.IsNullOrEmpty(certFile) || string.IsNullOrEmpty(priKeyPassword))
            {
                throw new ArgumentException("Certificate Credential arguments cannot be null");
            }
            this.usrName = usrName;
            this.pssWord = pasWord;
            this.certFile = certFile;
            this.priKeyPassword = priKeyPassword;
        }                    

        /// <summary>
        /// CertificateCredential constructor overload
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pasWord"></param>
        /// <param name="certFile"></param>
        /// <param name="priKeyPassword"></param>
        /// <param name="thrdPartyAuthorization"></param>
        public CertificateCredential(string usrName, string pssWord, string certFile, string priKeyPassword, 
            IThirdPartyAuthorization thrdPartyAuthorization)
            : this(usrName, pssWord, certFile, priKeyPassword)
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
        /// Gets and sets the Application ID (Used by Platform APIs)
        /// </summary>
        public string ApplicationID
        {
            get
            {
                return this.appID;
            }
            set
            {
                this.appID = value;
            }
        }

        /// <summary>
        /// Gets the Username credential
        /// </summary>
        public string UserName
        {
            get
            {
                return usrName;
            }
        }
       
        /// <summary>
        /// Gets the Password credential
        /// </summary>
        public string Password
        {
            get
            {
                return pssWord;
            }
        }

        /// <summary>
        /// Gets the File Name of the Certificate
        /// </summary>
        public string CertificateFile
        {
            get
            {
                return this.certFile;
            }
        }

        /// <summary>
        /// Gets the Password of the Certificate's Private Key
        /// </summary>
        public string PrivateKeyPassword
        {
            get
            {
                return this.priKeyPassword;
            }
        }
    }
}

