using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.NVP;
using PayPal.Authentication;

namespace PayPal.UnitTest.NVP
{
    [TestFixture]
    class SignatureHttpHeaderAuthStrategyTest
    {
        SignatureHttpHeaderAuthStrategy signHttpHeaderAuthStrategy;
        SignatureCredential signCredential;

        [Test]
        public void GenerateHeaderStrategyWithToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointNVP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(UnitTestConstants.AccessToken, UnitTestConstants.TokenSecret);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature", toknAuthorization);
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string authHeader = header[BaseConstants.PAYPAL_AUTHORIZATION_PLATFORM_HEADER];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + UnitTestConstants.AccessToken, headers[0]);
        }  

        [Test]
        public void GenerateHeaderStrategyWithoutToken()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointNVP);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string username = header[BaseConstants.PAYPAL_SECURITY_USERID_HEADER];
            string password = header[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER];
            string sign = header[BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.AreEqual("testsignature", sign);
        }
    }
}
