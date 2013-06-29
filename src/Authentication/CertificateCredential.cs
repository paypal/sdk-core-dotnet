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
        private string userNameCredential;

        /// <summary>
        /// Password credential
        /// </summary>
        private string passwordCredential;              

        /// <summary>
        /// Certificate file
        /// </summary>
        private string fileCertificate;

        /// <summary>
        /// Password of the Certificate's Private Key
        /// </summary>
        private string pvtKeyPassword;

        /// <summary>
        /// Third Party Authorization
        /// </summary>
        private IThirdPartyAuthorization authorization;

        /// <summary>
        ///  Application Id
        /// </summary>
        private string appId;

        /// <summary>
        /// CertificateCredential constructor
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="passwordCredential"></param>
        /// <param name="certFile"></param>
        /// <param name="pvtKeyPassword"></param>
        public CertificateCredential(string userNameCredential, string passwordCredential, string certFile, string pvtKeyPassword)
            : base()
        {
            if (string.IsNullOrEmpty(userNameCredential) || string.IsNullOrEmpty(passwordCredential) ||
                string.IsNullOrEmpty(certFile) || string.IsNullOrEmpty(pvtKeyPassword))
            {
                throw new ArgumentException("Certificate Credential arguments cannot be null");
            }
            this.userNameCredential = userNameCredential;
            this.passwordCredential = passwordCredential;
            this.fileCertificate = certFile;
            this.pvtKeyPassword = pvtKeyPassword;
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
        /// Gets the Username
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
        /// Gets the File Name of the Certificate
        /// </summary>
        public string CertificateFile
        {
            get
            {
                return this.fileCertificate;
            }
        }

        /// <summary>
        /// Gets the Password of the Certificate's Private Key
        /// </summary>
        public string PrivateKeyPassword
        {
            get
            {
                return this.pvtKeyPassword;
            }
        }
    }
}

