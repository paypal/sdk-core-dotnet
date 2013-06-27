using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal
{
    [TestClass]
    public class CredentialManagerTest
    {
        private CredentialManager CredentialMngr;
        private ICredential Credential;

        [TestMethod]
        public void LoadSignatureCredential()
        {
            string apiUsername = Constants.APIUserName;
            CredentialMngr = CredentialManager.Instance;
            Credential = CredentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.IsNotNull(Credential);
            Assert.IsInstanceOfType(Credential, typeof(SignatureCredential));
            SignatureCredential signCredential = (SignatureCredential) Credential;
            Assert.AreEqual(apiUsername, signCredential.UserName);
            Assert.AreEqual(Constants.APIPassword, signCredential.Password);
            Assert.AreEqual(Constants.APISignature, signCredential.Signature);
            Assert.AreEqual(Constants.ApplicationID, signCredential.ApplicationID);            
        }

        [TestMethod]
        public void LoadCertificateCredential()
        {
            string apiUsername = Constants.CertificateAPIUserName;
            CredentialMngr = CredentialManager.Instance;
            Credential = CredentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.IsNotNull(Credential);
            Assert.IsInstanceOfType(Credential, typeof(CertificateCredential));
            CertificateCredential certCredential = (CertificateCredential)Credential;
            Assert.AreEqual(apiUsername, certCredential.UserName);
            Assert.AreEqual(Constants.CertificateAPIPassword, certCredential.Password);
            Assert.AreEqual(Constants.CertificatePath, certCredential.CertificateFile);
            Assert.AreEqual(Constants.CertificatePassword, certCredential.PrivateKeyPassword);
            Assert.AreEqual(Constants.ApplicationID, certCredential.ApplicationID);
        }

        [TestMethod]
        [ExpectedException( typeof(MissingCredentialException))]
        public void LoadCredentialForNonExistentAccount()
        {
            try
            {
                CredentialMngr = CredentialManager.Instance;
                Credential = CredentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), "i-do-not-exist_api1.paypal.com");
            }
            catch (MissingCredentialException ex)
            {
                Assert.AreEqual("Missing credentials for i-do-not-exist_api1.paypal.com", ex.Message);
                throw;
            }           

        }
    }
}
