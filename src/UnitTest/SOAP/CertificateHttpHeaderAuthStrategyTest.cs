using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.SOAP;
using PayPal.Authentication;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class CertificateHttpHeaderAuthStrategyTest
    {
        CertificateHttpHeaderAuthStrategy certHttpHeaderAuthStrategy;
        CertificateCredential certCredential;

        [Test]
        public void GenerateHeaderStrategyWithToken()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointSOAP);                                 
            TokenAuthorization toknAuthorization = new TokenAuthorization(UnitTestConstants.AccessToken, UnitTestConstants.TokenSecret);
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y", toknAuthorization);
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            string authHeader = header[BaseConstants.PAYPAL_AUTHORIZATION_MERCHANT_HEADER];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + UnitTestConstants.AccessToken, headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutToken()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(UnitTestConstants.APIEndpointNVP);
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);            
            string username = header[BaseConstants.PAYPAL_SECURITY_USERID_HEADER];
            string password = header[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
        }
    }
}
