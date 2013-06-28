using PayPal.Manager;
using PayPal.Authentication;
using PayPal.Exception;

#if NUnit
using NUnit.Framework;

namespace PayPal.NUnitTest
{
    class CredentialManagerTest
    {
        CredentialManager credentialMngr;
        ICredential credential;

        [Test]
        public void LoadSignatureCredential()
        {
            string apiUsername = Constants.APIUserName;
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.NotNull(credential);
            Assert.IsInstanceOf(typeof(SignatureCredential), credential);
            SignatureCredential signCredential = (SignatureCredential)credential;
            Assert.AreEqual(apiUsername, signCredential.UserName);
            Assert.AreEqual(Constants.APIPassword, signCredential.Password);
            Assert.AreEqual(Constants.APISignature, signCredential.Signature);
            Assert.AreEqual(Constants.ApplicationID, signCredential.ApplicationID);
        }

        [Test]
        public void LoadCertificateCredential()
        {
            string apiUsername = Constants.CertificateAPIUserName;
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.NotNull(credential);
            Assert.IsInstanceOf(typeof(CertificateCredential), credential);
            CertificateCredential certCredential = (CertificateCredential)credential;
            Assert.AreEqual(apiUsername, certCredential.UserName);
            Assert.AreEqual(Constants.CertificateAPIPassword, certCredential.Password);
            Assert.AreEqual(Constants.CertificatePath, certCredential.CertificateFile);
            Assert.AreEqual(Constants.CertificatePassword, certCredential.PrivateKeyPassword);
            Assert.AreEqual(Constants.ApplicationID, certCredential.ApplicationID);
        }

        [Test, ExpectedException(typeof(MissingCredentialException))]
        public void LoadCredentialForNonExistentAccount()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), "i-do-not-exist_api1.paypal.com");
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
{
    [TestClass]
    public class CredentialManagerTest
    {
        private CredentialManager credentialMngr;
        private ICredential credential;

        [TestMethod]
        public void LoadSignatureCredential()
        {
            string apiUsername = Constants.APIUserName;
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.IsNotNull(credential);
            Assert.IsInstanceOfType(credential, typeof(SignatureCredential));
            SignatureCredential signCredential = (SignatureCredential)credential;
            Assert.AreEqual(apiUsername, signCredential.UserName);
            Assert.AreEqual(Constants.APIPassword, signCredential.Password);
            Assert.AreEqual(Constants.APISignature, signCredential.Signature);
            Assert.AreEqual(Constants.ApplicationID, signCredential.ApplicationID);
        }

        [TestMethod]
        public void LoadCertificateCredential()
        {
            string apiUsername = Constants.CertificateAPIUserName;
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), apiUsername);
            Assert.IsNotNull(credential);
            Assert.IsInstanceOfType(credential, typeof(CertificateCredential));
            CertificateCredential certCredential = (CertificateCredential)credential;
            Assert.AreEqual(apiUsername, certCredential.UserName);
            Assert.AreEqual(Constants.CertificateAPIPassword, certCredential.Password);
            Assert.AreEqual(Constants.CertificatePath, certCredential.CertificateFile);
            Assert.AreEqual(Constants.CertificatePassword, certCredential.PrivateKeyPassword);
            Assert.AreEqual(Constants.ApplicationID, certCredential.ApplicationID);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingCredentialException))]
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
#endif
