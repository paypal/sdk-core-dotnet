using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.SOAP;
using PayPal.Authentication;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class SignatureHttpHeaderAuthStrategyTest
    {
        SignatureHttpHeaderAuthStrategy signHttpHeaderAuthStrategy;
        SignatureCredential signCredential;

        [Test]
        public void GenerateHeaderStrategyWithToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointSOAP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(UnitTestConstants.AccessToken, UnitTestConstants.TokenSecret);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature", toknAuthorization);
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string authHeader = header[BaseConstants.PAYPAL_AUTHORIZATION_MERCHANT_HEADER];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + UnitTestConstants.AccessToken, headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointSOAP);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);            
            string username = header[BaseConstants.PAYPAL_SECURITY_USERID_HEADER];
            string password = header[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER];
            string sign = header[BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.AreEqual("testsignature", sign);
        }
    }
}
