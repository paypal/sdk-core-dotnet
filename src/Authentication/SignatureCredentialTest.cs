using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Authentication;

namespace PayPal
{
    [TestClass]
    public class SignatureCredentialTest
    {
        private SignatureCredential SignCredential;

        public SignatureCredentialTest()
        {
            SignCredential = new SignatureCredential("platfo_1255077030_biz_api1.gmail.com", "1255077037", "Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf");
        }

        [TestMethod]
        public void Signature()
        {
            Assert.AreEqual("Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf", ((SignatureCredential)SignCredential).Signature);
        }

        [TestMethod]
        public void Password()
        {
            Assert.AreEqual("1255077037", SignCredential.Password);
        }

        [TestMethod]
        public void UserName()
        {
            Assert.AreEqual("platfo_1255077030_biz_api1.gmail.com", SignCredential.UserName);
        }

        [TestMethod]
        public void ApplicationID()
        {
            SignCredential.ApplicationID = Constants.ApplicationID;
            Assert.AreEqual(Constants.ApplicationID, SignCredential.ApplicationID);
        }

        [TestMethod]
        public void ThirdPartyAuthorizationForSubject()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new SubjectAuthorization("Subject");
            SignCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((SubjectAuthorization)thirdPartyAuthorization).Subject, "Subject");
        }

        [TestMethod]
        public void ThirdPartyAuthorizationForToken()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            SignCredential.ThirdPartyAuthorization = thirdPartyAuthorization;            
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessToken, Constants.AccessToken);
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessTokenSecret, Constants.TokenSecret);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SignatureCredentialArgumentException()
        {
            try
            {
                SignCredential = new SignatureCredential(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Signature Credential arguments cannot be null", ex.Message);
                throw;
            }
        }
    }
}
