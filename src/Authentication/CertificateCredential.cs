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
        private string UsrName;

        /// <summary>
        /// Password credential
        /// </summary>
        private string PssWord;              

        /// <summary>
        /// Certificate file
        /// </summary>
        private string CertFile;

        /// <summary>
        /// Password of the Certificate's Private Key
        /// </summary>
        private string PriKeyPassword;

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
            this.UsrName = usrName;
            this.PssWord = pasWord;
            this.CertFile = certFile;
            this.PriKeyPassword = priKeyPassword;
        }                    

        /// <summary>
        /// CertificateCredential constructor overload
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pasWord"></param>
        /// <param name="certFile"></param>
        /// <param name="priKeyPassword"></param>
        /// <param name="thrdPartyAuthorization"></param>
        public CertificateCredential(string usrName, string pasWord, string certFile, string priKeyPassword, 
            IThirdPartyAuthorization thrdPartyAuthorization)
            : this(usrName, pasWord, certFile, priKeyPassword)
        {
            this.ThirdPartyAuthorization = thrdPartyAuthorization;
        }   
        
        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Username credential
        /// </summary>
        public string UserName
        {
            get
            {
                return UsrName;
            }
        }
       
        /// <summary>
        /// Gets the Password credential
        /// </summary>
        public string Password
        {
            get
            {
                return PssWord;
            }
        }

        /// <summary>
        /// Gets and sets the Application ID (Used by Platform APIs)
        /// </summary>
        public string ApplicationID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the File Name of the Certificate
        /// </summary>
        public string CertificateFile
        {
            get
            {
                return this.CertFile;
            }
        }

        /// <summary>
        /// Gets the Password of the Certificate's Private Key
        /// </summary>
        public string PrivateKeyPassword
        {
            get
            {
                return this.PriKeyPassword;
            }
        }
    }
}

