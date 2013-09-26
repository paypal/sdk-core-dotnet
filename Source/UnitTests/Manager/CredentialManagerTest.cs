using System.IO;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.Exception;

#if NUnit
/* NuGet Install
 * Visual Studio 2005
    * Install NUnit -OutputDirectory .\packages
    * Add reference from NUnit.2.6.2
 */
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
            Assert.AreEqual(Constants.ApplicationId, signCredential.ApplicationId);
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
            Assert.AreEqual(Path.GetFileName(Constants.CertificatePath), Path.GetFileName(certCredential.CertificateFile));
            Assert.AreEqual(Constants.CertificatePassword, certCredential.PrivateKeyPassword);
            Assert.AreEqual(Constants.ApplicationId, certCredential.ApplicationId);
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
            Assert.AreEqual(Constants.ApplicationId, signCredential.ApplicationId);
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
            Assert.AreEqual(Path.GetFileName(Constants.CertificatePath), Path.GetFileName(certCredential.CertificateFile));
            Assert.AreEqual(Constants.CertificatePassword, certCredential.PrivateKeyPassword);
            Assert.AreEqual(Constants.ApplicationId, certCredential.ApplicationId);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingCredentialException), "Missing credentials for i-do-not-exist_api1.paypal.com")]
        public void LoadCredentialForNonExistentAccount()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), "i-do-not-exist_api1.paypal.com");
        }
    }
}
#endif
