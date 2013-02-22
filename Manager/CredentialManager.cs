using log4net;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal.Manager
{
    /// <summary>
    /// Reads API credentials to be used with the application
    /// </summary>
    public sealed class CredentialManager
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog logger = LogManagerWrapper.GetLogger(typeof(CredentialManager));

        private readonly ConfigManager configMgr;

        /// <summary>
        /// Private constructor
        /// </summary>
        public CredentialManager(ConfigManager configMgr)
        {
            this.configMgr = configMgr;
        }

        /// <summary>
        /// Returns the default Account Name
        /// </summary>
        /// <returns></returns>
        private string GetDefaultAccountName()
        {
            Account firstAccount = configMgr.GetAccount(0);
            if (firstAccount == null)
            {
                throw new MissingCredentialException("No accounts configured for API call");
            }
            return firstAccount.APIUsername;
        }

        /// <summary>
        /// Returns the API Credentials
        /// </summary>
        /// <param name="apiUserName"></param>
        /// <returns></returns>
        public ICredential GetCredentials(string apiUserName)
        {
            if (apiUserName == null)
            {
                apiUserName = GetDefaultAccountName();
            }
           
            ICredential credential = null;
            Account accnt = configMgr.GetAccount(apiUserName);
            if (accnt == null)
            {
                throw new MissingCredentialException("Missing credentials for " + apiUserName);
            }
            if (!string.IsNullOrEmpty(accnt.APICertificate))
            {
                CertificateCredential certCredential = new CertificateCredential(accnt.APIUsername, accnt.APIPassword, accnt.APICertificate, accnt.PrivateKeyPassword);
                certCredential.ApplicationID = accnt.ApplicationId;
                if (!string.IsNullOrEmpty(accnt.CertificateSubject))
                {
                    SubjectAuthorization subAuthorization = new SubjectAuthorization(accnt.CertificateSubject);
                    certCredential.ThirdPartyAuthorization = subAuthorization;
                }
                credential = certCredential;
            }
            else
            {
                SignatureCredential signCredential = new SignatureCredential(accnt.APIUsername, accnt.APIPassword, accnt.APISignature);
                signCredential.ApplicationID = accnt.ApplicationId;
                if (!string.IsNullOrEmpty(accnt.SignatureSubject))
                {
                    SubjectAuthorization subjectAuthorization = new SubjectAuthorization(accnt.SignatureSubject);
                    signCredential.ThirdPartyAuthorization = subjectAuthorization;
                }
                credential = signCredential;
            }
            ValidateCredentials(credential);
            
            return credential;            
        }

        /// <summary>
        /// Validates the API Credentials
        /// </summary>
        /// <param name="apiCredentials"></param>
        private void ValidateCredentials(ICredential apiCredentials)
        {
            if (apiCredentials is SignatureCredential)
            {
                SignatureCredential credential = (SignatureCredential)apiCredentials;
                Validate(credential);
            }
            else if (apiCredentials is CertificateCredential)
            {
                CertificateCredential credential = (CertificateCredential)apiCredentials;
                Validate(credential);
            }
        }

        /// <summary>
        /// Validates the Signature Credentials
        /// </summary>
        /// <param name="apiCredentials"></param>
        private void Validate(SignatureCredential apiCredentials)
        {
            if (string.IsNullOrEmpty(apiCredentials.UserName))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.err_username);
            }
            if (string.IsNullOrEmpty(apiCredentials.Password))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.err_passeword);
            }
            if (string.IsNullOrEmpty(((SignatureCredential)apiCredentials).Signature))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.err_signature);
            }
        }

        /// <summary>
        /// Validates the Certificate Credentials
        /// </summary>
        /// <param name="apiCredentials"></param>
        private void Validate(CertificateCredential apiCredentials)
        {
            if (string.IsNullOrEmpty(apiCredentials.UserName))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.err_username);
            }
            if (string.IsNullOrEmpty(apiCredentials.Password))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.err_passeword);
            }

            if (string.IsNullOrEmpty(((CertificateCredential)apiCredentials).CertificateFile))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.err_certificate);
            }

            if (string.IsNullOrEmpty(((CertificateCredential)apiCredentials).PrivateKeyPassword))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.err_privatekeypassword);
            }
        }      
    }
}
