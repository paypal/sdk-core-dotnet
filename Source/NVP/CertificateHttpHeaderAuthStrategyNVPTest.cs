using System.Collections.Generic;
using PayPal.NVP;
using PayPal.Authentication;

#if NUnit
/* NuGet Install
 * Visual Studio 2005
    * Install NUnit -OutputDirectory .\packages
    * Add reference from NUnit.2.6.2
 */
using NUnit.Framework;

namespace PayPal.NUnitTest
{
    [TestFixture]
    class CertificateHttpHeaderAuthStrategyNVPTest
    {
        CertificateHttpHeaderAuthStrategy certHttpHeaderAuthStrategy;
        CertificateCredential certCredential;

        [Test]
        public void GenerateHeaderStrategyWithTokenTest()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(Constants.APIEndpointSOAP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y", toknAuthorization);
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + Constants.AccessToken, headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutTokenTest()
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
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
{
    [TestClass]
    public class CertificateHttpHeaderAuthStrategyNVPTest
    {
        private CertificateHttpHeaderAuthStrategy certHttpHeaderAuthStrategy;
        private CertificateCredential certCredential;

        [TestMethod]
        public void GenerateHeaderStrategyWithTokenTest()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(Constants.APIEndpointSOAP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y", toknAuthorization);
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + Constants.AccessToken, headers[0]);
        }

        [TestMethod]
        public void GenerateHeaderStrategyWithoutTokenTest()
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
#endif
