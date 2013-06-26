using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal
{
    [TestClass]
    public class CredentialManagerTest
    {
        CredentialManager credentialMngr;
        ICredential credential;

        [TestMethod]
        public void LoadSignatureCredential()
        {
            string apiUsername = UnitTestConstants.APIUserName;
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.IsNotNull(credential);
            Assert.IsInstanceOfType(credential, typeof(SignatureCredential));
            SignatureCredential signCredential = (SignatureCredential) credential;
            Assert.AreEqual(apiUsername, signCredential.UserName);
            Assert.AreEqual(UnitTestConstants.APIPassword, signCredential.Password);
            Assert.AreEqual(UnitTestConstants.APISignature, signCredential.Signature);
            Assert.AreEqual(UnitTestConstants.ApplicationID, signCredential.ApplicationID);            
        }

        [TestMethod]
        public void LoadCertificateCredential()
        {
            string apiUsername = UnitTestConstants.CertificateAPIUserName;
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.IsNotNull(credential);
            Assert.IsInstanceOfType(credential, typeof(CertificateCredential));
            CertificateCredential certCredential = (CertificateCredential)credential;
            Assert.AreEqual(apiUsername, certCredential.UserName);
            Assert.AreEqual(UnitTestConstants.CertificateAPIPassword, certCredential.Password);
            Assert.AreEqual(UnitTestConstants.CertificatePath, certCredential.CertificateFile);
            Assert.AreEqual(UnitTestConstants.CertificatePassword, certCredential.PrivateKeyPassword);
            Assert.AreEqual(UnitTestConstants.ApplicationID, certCredential.ApplicationID);
        }

        [TestMethod]
        [ExpectedException( typeof(MissingCredentialException))]
        public void LoadCredentialForNonExistentAccount()
        {
            try
            {
                credentialMngr = CredentialManager.Instance;
                credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), "i-do-not-exist_api1.paypal.com");
            }
            catch (MissingCredentialException ex)
            {
                Assert.AreEqual("Missing credentials for i-do-not-exist_api1.paypal.com", ex.Message);
                throw;
            }           

        }
    }
}
