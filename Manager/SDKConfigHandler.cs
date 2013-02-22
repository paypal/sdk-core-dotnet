using System.Collections.Generic;

namespace PayPal.Manager
{
    /// <summary>
    /// Custom handler for SDK configuration section as defined in App.Config or Web.Config files
    /// </summary>    
    public class SDKConfigHandler
    {
        private readonly Dictionary<string, string> _settings;
        private readonly Dictionary<string, Account> _accounts;

        public SDKConfigHandler(Dictionary<string, string> settings, Dictionary<string, Dictionary<string, string>> accounts)
        {
            _settings = settings;
            _accounts = new Dictionary<string, Account>();

            foreach (var entry in accounts)
            {
                var apiUserName = entry.Key;

                string apiPassword;
                entry.Value.TryGetValue("apiPassword", out apiPassword);

                string apiSignature;
                entry.Value.TryGetValue("apiSignature", out apiSignature);

                string applicationId;
                entry.Value.TryGetValue("applicationId", out applicationId);

                string apiCertificate;
                entry.Value.TryGetValue("apiCertificate", out apiCertificate);

                string privateKeyPassword;
                entry.Value.TryGetValue("privateKeyPassword", out privateKeyPassword);

                string signatureSubject;
                entry.Value.TryGetValue("signatureSubject", out signatureSubject);

                string certificateSubject;
                entry.Value.TryGetValue("certificateSubject", out certificateSubject);

                _accounts.Add(apiUserName, new Account(apiUserName, apiPassword, apiSignature, applicationId, apiCertificate, privateKeyPassword, signatureSubject));
            }
        }

        /// <summary>
        /// Accounts Collection
        /// </summary>
        public Dictionary<string, Account> Accounts
        {
            get { return _accounts; }
        }

        public string Setting(string name)
        {
            var value = string.Empty;
            return _settings.TryGetValue(name, out value) ? value : null;
        }
    }    

    /// <summary>
    /// Class holds the <Account> element
    /// </summary>
    public class Account
    {
        public string APIUsername { get; set; }
        public string APIPassword { get; set; }
        public string APISignature { get; set; }
        public string ApplicationId { get; set; }
        public string APICertificate { get; set; }
        public string PrivateKeyPassword { get; set; }
        public string SignatureSubject { get; set; }
        public string CertificateSubject { get; set; }

        public Account(string apiUsername, string apiPassword, string apiSignature, string applicationId = "", string apiCertificate = "", string privateKeyPassword = "", string signatureSubject = "", string certificateSubject = "")
        {
            APIUsername = apiUsername;
            APIPassword = apiPassword;
            APISignature = apiSignature;
            ApplicationId = applicationId;
            APICertificate = apiCertificate;
            PrivateKeyPassword = privateKeyPassword;
            SignatureSubject = signatureSubject;
            CertificateSubject = certificateSubject;
        }
    }   
}
