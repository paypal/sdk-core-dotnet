using System;
using PayPal.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
{
    [TestClass]
    public class CertificateCredentialTest
    {
        private CertificateCredential certCredential;

        public CertificateCredentialTest()
        {
            certCredential = new CertificateCredential("platfo_1255077030_biz_api1.gmail.com", "1255077037", "sdk-cert.p12", "KJAERUGBLVF6Y");
        }

        [TestMethod]
        public void UserName()
        {
            Assert.AreEqual("platfo_1255077030_biz_api1.gmail.com", certCredential.UserName);
        }

        [TestMethod]
        public void Password()
        {
            Assert.AreEqual("1255077037", certCredential.Password);
        }

        [TestMethod]
        public void CertificateFile()
        {
            Assert.AreEqual("sdk-cert.p12", certCredential.CertificateFile);
        }

        [TestMethod]
        public void PrivateKeyPassword()
        {
            Assert.AreEqual("KJAERUGBLVF6Y", certCredential.PrivateKeyPassword);
        }

        [TestMethod]
        public void ApplicationId()
        {
            certCredential.ApplicationId = Constants.ApplicationId;
            Assert.AreEqual(Constants.ApplicationId, certCredential.ApplicationId);
        }

        [TestMethod]
        public void SetAndGetThirdPartyAuthorizationForSubject()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new SubjectAuthorization("Subject");
            certCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((SubjectAuthorization)thirdPartyAuthorization).Subject, "Subject");

        }

        [TestMethod]
        public void ThirdPartyAuthorizationTestForToken()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            certCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessToken, Constants.AccessToken);
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessTokenSecret, Constants.TokenSecret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Certificate Credential arguments cannot be null")]
        public void CertificateCredentialArgumentException()
        {
            certCredential = new CertificateCredential(null, null, null, null);            
        }
    }
}