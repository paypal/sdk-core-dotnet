using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;
using PayPal.Log;

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
        private static Logger logger = Logger.GetLogger(typeof(CredentialManager));
        
        private static string accountPrefix = "account";

#if NET_2_0 || NET_3_5
        /// <summary>
        /// Singleton instance of ConnectionManager
        /// </summary>
        private static readonly CredentialManager singletonInstance = new CredentialManager();

        /// <summary>
        /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// </summary>
        static CredentialManager() { }

        /// <summary>
        /// Gets the Singleton instance of ConnectionManager
        /// </summary>
        public static CredentialManager Instance
        {
            get
            {
                return singletonInstance;
            }
        }
#elif NET_4_0 || NET_4_5 || NET_4_5_1
        /// <summary>
        /// System.Lazy type guarantees thread-safe lazy-construction
        /// static holder for instance, need to use lambda to construct since constructor private
        /// </summary>
        private static readonly Lazy<CredentialManager> laze = new Lazy<CredentialManager>(() => new CredentialManager());

        /// <summary>
        /// Accessor for the Singleton instance of ConnectionManager
        /// </summary>
        public static CredentialManager Instance { get { return laze.Value; } }    
#endif

        /// <summary>
        /// Private constructor, private to prevent direct instantiation
        /// </summary>
        private CredentialManager() { }   

        /// <summary>
        /// Returns the default Account Name
        /// </summary>
        /// <returns></returns>
        private Account GetAccount(Dictionary<string, string> config, string apiUserName)
        {                        
            foreach (KeyValuePair<string, string> kvPair in config)
            {
                //logger.Info(kvPair.Key + " " + kvPair.Value);
                if(kvPair.Key.EndsWith(".apiUsername"))
                {
                    if (apiUserName == null || apiUserName.Equals(kvPair.Value)) 
                    {
                        int index = Convert.ToInt32(kvPair.Key.Substring(accountPrefix.Length, kvPair.Key.IndexOf('.') - accountPrefix.Length ));
                        Account accnt = new Account();
                        if (config.ContainsKey(accountPrefix +  index + ".apiUsername")) 
                        {
                            accnt.APIUserName = config[accountPrefix +  index + ".apiUsername"];
                        }
                        if(config.ContainsKey(accountPrefix +  index + ".apiPassword"))
                        {
                            accnt.APIPassword = config[accountPrefix +  index + ".apiPassword"];
                        }
                        if(config.ContainsKey(accountPrefix +  index + ".apiSignature")) 
                        {
                            accnt.APISignature = config[accountPrefix +  index + ".apiSignature"];
                        }
                        if(config.ContainsKey(accountPrefix +  index + ".apiCertificate")) 
                        {
                            accnt.APICertificate = config[accountPrefix +  index + ".apiCertificate"];
                        }
                        if (config.ContainsKey(accountPrefix +  index + ".privateKeyPassword")) 
                        {
                            accnt.PrivateKeyPassword = config[accountPrefix +  index + ".privateKeyPassword"];
                        }            
                        if(config.ContainsKey(accountPrefix +  index + ".subject"))
                        {
                            accnt.CertificateSubject = config[accountPrefix +  index + ".subject"];
                        }
                        if(config.ContainsKey(accountPrefix +  index + ".applicationId"))
                        {
                            accnt.ApplicationId = config[accountPrefix +  index + ".applicationId"];
                        }
                        return accnt;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the API Credentials
        /// </summary>
        /// <param name="apiUserName"></param>
        /// <returns></returns>
        public ICredential GetCredentials(Dictionary<string, string> config, string apiUserName)
        {
            ICredential credential = null;
            Account accnt = GetAccount(config, apiUserName);
            if (accnt == null)
            {
                throw new MissingCredentialException("Missing credentials for " + apiUserName);
            }
            if (!string.IsNullOrEmpty(accnt.APICertificate))
            {
                CertificateCredential certCredential = new CertificateCredential(accnt.APIUserName, accnt.APIPassword, accnt.APICertificate, accnt.PrivateKeyPassword);
                certCredential.ApplicationId = accnt.ApplicationId;
                if (!string.IsNullOrEmpty(accnt.CertificateSubject))
                {
                    SubjectAuthorization subAuthorization = new SubjectAuthorization(accnt.CertificateSubject);
                    certCredential.ThirdPartyAuthorization = subAuthorization;
                }
                credential = certCredential;
            }
            else
            {
                SignatureCredential signCredential = new SignatureCredential(accnt.APIUserName, accnt.APIPassword, accnt.APISignature);
                signCredential.ApplicationId = accnt.ApplicationId;
                if (!string.IsNullOrEmpty(accnt.SignatureSubject))
                {
                    SubjectAuthorization subjectAuthorization = new SubjectAuthorization(accnt.SignatureSubject);
                    signCredential.ThirdPartyAuthorization = subjectAuthorization;
                }
                if (!string.IsNullOrEmpty(accnt.CertificateSubject))
                {
                    SubjectAuthorization subAuthorization = new SubjectAuthorization(accnt.CertificateSubject);
                    signCredential.ThirdPartyAuthorization = subAuthorization;
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
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.ErrorUserName);
            }
            if (string.IsNullOrEmpty(apiCredentials.Password))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.ErrorPassword);
            }
            if (string.IsNullOrEmpty(((SignatureCredential)apiCredentials).Signature))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.ErrorSignature);
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
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.ErrorUserName);
            }
            if (string.IsNullOrEmpty(apiCredentials.Password))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.ErrorPassword);
            }

            if (string.IsNullOrEmpty(((CertificateCredential)apiCredentials).CertificateFile))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.ErrorCertificate);
            }

            if (string.IsNullOrEmpty(((CertificateCredential)apiCredentials).PrivateKeyPassword))
            {
                throw new InvalidCredentialException(BaseConstants.ErrorMessages.ErrorPrivateKeyPassword);
            }
        }      
    }
}
