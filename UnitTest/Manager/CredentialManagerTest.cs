using System;
using NUnit.Framework;
using PayPal.Exception;
using PayPal.Manager;
using PayPal.Authentication;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    class CredentialManagerTest : TestsBase
    {
        ICredential credential;

        [Test]
        public void LoadSignatureCredential()
        {
            string apiUsername = UnitTestConstants.APIUserName;
            credential = AppCredentialMgr.GetCredentials(apiUsername);
            Assert.NotNull(credential);
            Assert.IsInstanceOf(typeof(SignatureCredential), credential);
            SignatureCredential signCredential = (SignatureCredential) credential;
            Assert.AreEqual(apiUsername, signCredential.UserName);
            Assert.AreEqual(UnitTestConstants.APIPassword, signCredential.Password);
            Assert.AreEqual(UnitTestConstants.APISignature, signCredential.Signature);
            Assert.AreEqual(UnitTestConstants.ApplicationID, signCredential.ApplicationID);            
        }

        [Test]
        public void LoadCertificateCredential()
        {
            string apiUsername = UnitTestConstants.CertificateAPIUserName;
            credential = AppCredentialMgr.GetCredentials(apiUsername);
            Assert.NotNull(credential);
            Assert.IsInstanceOf(typeof(CertificateCredential), credential);
            CertificateCredential certCredential = (CertificateCredential)credential;
            Assert.AreEqual(apiUsername, certCredential.UserName);
            Assert.AreEqual(UnitTestConstants.CertificateAPIPassword, certCredential.Password);
            Assert.AreEqual(UnitTestConstants.CertificatePath, certCredential.CertificateFile);
            Assert.AreEqual(UnitTestConstants.CertificatePassword, certCredential.PrivateKeyPassword);
            Assert.AreEqual(UnitTestConstants.ApplicationID, certCredential.ApplicationID);
        }

        [Test, ExpectedException( typeof(MissingCredentialException) )]
        public void LoadCredentialForNonExistentAccount()
        {
            credential = AppCredentialMgr.GetCredentials("i-do-not-exist_api1.paypal.com");
        }
    }
}
