using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace PayPal.Manager.AppConfig
{
    /// <summary>
    /// Custom handler for SDK configuration section as defined in App.Config or Web.Config files
    /// </summary>    
    public class SDKAppConfigHandler : ConfigurationSection 
    {
        private static readonly ConfigurationProperty accountsElement =
             new ConfigurationProperty("accounts", typeof(AccountCollection), null, ConfigurationPropertyOptions.IsRequired);

        /// <summary>
        /// Accounts Collection
        /// </summary>
        [ConfigurationProperty("accounts", IsRequired = true)]
        public AccountCollection Accounts
        {
            get { return (AccountCollection)this[accountsElement]; }
        }

        [ConfigurationProperty("settings", IsRequired = true)]
        private NameValueConfigurationCollection Settings
        {
            get { return (NameValueConfigurationCollection)this["settings"]; }
        }

        public string Setting(string name)
        {
            NameValueConfigurationElement config = Settings[name];
            return ((config == null) ? null : config.Value);
        }
    }

    [ConfigurationCollection(typeof(IAccount), AddItemName = "account",
         CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class AccountCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppAccount();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppAccount)element).APIUsername;
        }

        public AppAccount Account(int index)
        {
            return (AppAccount)BaseGet(index);
        }

        public AppAccount Account(string value)
        {
            return (AppAccount)BaseGet(value);
        }

        public new AppAccount this[string name]
        {
            get { return (AppAccount)BaseGet(name); }
        }

        public AppAccount this[int index]
        {
            get { return (AppAccount)BaseGet(index); }
        }
    }

    /// <summary>
    /// Class holds the <Account> element
    /// </summary>
    public class AppAccount : ConfigurationElement, IAccount
    {
        private static readonly ConfigurationProperty apiUsername =
            new ConfigurationProperty("apiUsername", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty apiPassword =
            new ConfigurationProperty("apiPassword", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty applicationId =
            new ConfigurationProperty("applicationId", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty apiSignature =
            new ConfigurationProperty("apiSignature", typeof(string), string.Empty);

        private static readonly ConfigurationProperty apiCertificate =
            new ConfigurationProperty("apiCertificate", typeof(string), string.Empty);

        private static readonly ConfigurationProperty privateKeyPassword =
            new ConfigurationProperty("privateKeyPassword", typeof(string), string.Empty);

        private static readonly ConfigurationProperty signSubject =
           new ConfigurationProperty("signatureSubject", typeof(string), string.Empty);

        private static readonly ConfigurationProperty certifySubject =
           new ConfigurationProperty("certificateSubject", typeof(string), string.Empty);

        public AppAccount()
        {
            base.Properties.Add(apiUsername);
            base.Properties.Add(apiPassword);
            base.Properties.Add(applicationId);
            base.Properties.Add(apiSignature);
            base.Properties.Add(apiCertificate);
            base.Properties.Add(privateKeyPassword);
            base.Properties.Add(signSubject);
            base.Properties.Add(certifySubject);
        }

        /// <summary>
        /// API Username
        /// </summary>
        [ConfigurationProperty("apiUsername", IsRequired = true)]
        public string APIUsername
        {
            get { return (string)this[apiUsername]; }
        }

        /// <summary>
        /// API password
        /// </summary>
        [ConfigurationProperty("apiPassword", IsRequired = true)]
        public string APIPassword
        {
            get { return (string)this[apiPassword]; }
        }

        /// <summary>
        /// Application ID
        /// </summary>
        [ConfigurationProperty("applicationId")]
        public string ApplicationId
        {
            get { return (string)this[applicationId]; }
        }

        /// <summary>
        /// API signature
        /// </summary>
        [ConfigurationProperty("apiSignature")]
        public string APISignature
        {
            get { return (string)this[apiSignature]; }
        }

        /// <summary>
        /// Client certificate for SSL authentication
        /// </summary>
        [ConfigurationProperty("apiCertificate")]
        public string APICertificate
        {
            get { return (string)this[apiCertificate]; }
        }

        /// <summary>
        /// Private key password for SSL authentication
        /// </summary>
        [ConfigurationProperty("privateKeyPassword")]
        public string PrivateKeyPassword
        {
            get { return (string)this[privateKeyPassword]; }
        }

        /// <summary>
        /// Signature Subject
        /// </summary>
        [ConfigurationProperty("signatureSubject")]
        public string SignatureSubject
        {
            get { return (string)this[signSubject]; }
        }

        /// <summary>
        /// Certificate Subject
        /// </summary>
        [ConfigurationProperty("certificateSubject")]
        public string CertificateSubject
        {
            get { return (string)this[certifySubject]; }
        }
    }
}