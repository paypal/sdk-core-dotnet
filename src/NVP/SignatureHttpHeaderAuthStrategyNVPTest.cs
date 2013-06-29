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
    class SignatureHttpHeaderAuthStrategyNVPTest
    {
        SignatureHttpHeaderAuthStrategy signHttpHeaderAuthStrategy;
        SignatureCredential signCredential;

        [Test]
        public void GenerateHeaderStrategyWithToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(Constants.APIEndpointNVP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature", toknAuthorization);
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + Constants.AccessToken, headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutToken()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(Constants.APIEndpointNVP);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string username = header[BaseConstants.PayPalSecurityUserIdHeader];
            string password = header[BaseConstants.PayPalSecurityPasswordHeader];
            string sign = header[BaseConstants.PayPalSecuritySignatureHeader];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.AreEqual("testsignature", sign);
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
{
    [TestClass]
    public class SignatureHttpHeaderAuthStrategyNVPTest
    {
        SignatureHttpHeaderAuthStrategy signHttpHeaderAuthStrategy;
        SignatureCredential signCredential;

        [TestMethod]
        public void GenerateHeaderStrategyWithToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(Constants.APIEndpointNVP);
            TokenAuthorization toknAuthorization = new TokenAuthorization(Constants.AccessToken, Constants.TokenSecret);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature", toknAuthorization);
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + Constants.AccessToken, headers[0]);
        }

        [TestMethod]
        public void GenerateHeaderStrategyWithoutToken()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(Constants.APIEndpointNVP);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string username = header[BaseConstants.PayPalSecurityUserIdHeader];
            string password = header[BaseConstants.PayPalSecurityPasswordHeader];
            string sign = header[BaseConstants.PayPalSecuritySignatureHeader];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.AreEqual("testsignature", sign);
        }
    }
}
#endif
