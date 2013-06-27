using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Authentication;

namespace PayPal
{
    [TestClass]
    public class SignatureCredentialTest
    {
        SignatureCredential signCredential;

        public SignatureCredentialTest()
        {
            signCredential = new SignatureCredential("platfo_1255077030_biz_api1.gmail.com", "1255077037", "Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf");
        }

        [TestMethod]
        public void Signature()
        {
            Assert.AreEqual("Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf", ((SignatureCredential)signCredential).Signature);
        }

        [TestMethod]
        public void Password()
        {
            Assert.AreEqual("1255077037", signCredential.Password);
        }

        [TestMethod]
        public void UserName()
        {
            Assert.AreEqual("platfo_1255077030_biz_api1.gmail.com", signCredential.UserName);
        }

        [TestMethod]
        public void ApplicationID()
        {
            signCredential.ApplicationID = Constants.ApplicationID;
            Assert.AreEqual(Constants.ApplicationID, signCredential.ApplicationID);
        }

        [TestMethod]
        public void ThirdPartyAuthorizationForSubject()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new SubjectAuthorization("Subject");
            signCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((SubjectAuthorization)thirdPartyAuthorization).Subject, "Subject");
        }

        [TestMethod]
        public void ThirdPartyAuthorizationForToken()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            signCredential.ThirdPartyAuthorization = thirdPartyAuthorization;            
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessToken, Constants.AccessToken);
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessTokenSecret, Constants.TokenSecret);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SignatureCredentialArgumentException()
        {
            try
            {
                signCredential = new SignatureCredential(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Signature Credential arguments cannot be null", ex.Message);
                throw;
            }
        }
    }
}
