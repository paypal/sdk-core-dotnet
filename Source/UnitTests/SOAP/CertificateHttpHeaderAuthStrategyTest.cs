using System.Collections.Generic;
using PayPal.SOAP;
using PayPal.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
{
    [TestClass]
    public class CertificateHttpHeaderAuthStrategyTest
    {
        CertificateHttpHeaderAuthStrategy certHttpHeaderAuthStrategy;
        CertificateCredential certCredential;

        [TestMethod]
        public void GenerateHeaderStrategyWithToken()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(Constants.APIEndpointSOAP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y", toknAuthorization);
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            string authHeader = header[BaseConstants.PayPalAuthorizationMerchantHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + Constants.AccessToken, headers[0]);
        }

        [TestMethod]
        public void GenerateHeaderStrategyWithoutToken()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(Constants.APIEndpointNVP);
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            string username = header[BaseConstants.PayPalSecurityUserIdHeader];
            string password = header[BaseConstants.PayPalSecurityPasswordHeader];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
        }
    }
}