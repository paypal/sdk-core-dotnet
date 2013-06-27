using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.NVP;
using PayPal.Authentication;

namespace PayPal
{
    [TestClass]
    public class CertificateHttpHeaderAuthStrategyTest
    {
        private CertificateHttpHeaderAuthStrategy CertHttpHeaderAuthStrategy;
        private CertificateCredential CertCredential;

        [TestMethod]
        public void GenerateHeaderStrategyWithTokenTest()
        {
            CertHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(Constants.APIEndpointSOAP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            CertCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y", toknAuthorization);
            Dictionary<string, string> header = CertHttpHeaderAuthStrategy.GenerateHeaderStrategy(CertCredential);            
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + Constants.AccessToken, headers[0]);
        }

        [TestMethod]
        public void GenerateHeaderStrategyWithoutTokenTest()
        {
            CertHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(Constants.APIEndpointNVP);
            CertCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            Dictionary<string, string> header = CertHttpHeaderAuthStrategy.GenerateHeaderStrategy(CertCredential);            
            string username = header[BaseConstants.PayPalSecurityUserIDHeader];
            string password = header[BaseConstants.PayPalSecurityPasswordHeader];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
        }       
    }
}
