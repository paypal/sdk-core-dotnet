using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.NVP;
using PayPal.Authentication;

namespace PayPal
{
    [TestClass]
    public class SignatureHttpHeaderAuthStrategyTest
    {
        SignatureHttpHeaderAuthStrategy signHttpHeaderAuthStrategy;
        SignatureCredential signCredential;

        [TestMethod]
        public void GenerateHeaderStrategyWithToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointNVP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(UnitTestConstants.AccessToken, UnitTestConstants.TokenSecret);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature", toknAuthorization);
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + UnitTestConstants.AccessToken, headers[0]);
        }  

        [TestMethod]
        public void GenerateHeaderStrategyWithoutToken()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointNVP);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string username = header[BaseConstants.PayPalSecurityUserIDHeader];
            string password = header[BaseConstants.PayPalSecurityPasswordHeader];
            string sign = header[BaseConstants.PayPalSecuritySignatureHeader];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.AreEqual("testsignature", sign);
        }
    }
}
