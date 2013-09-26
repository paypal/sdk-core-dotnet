using System;
using System.Collections.Generic;
using PayPal;

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
    class OAuthTokenCredentialTest
    {
        /// <summary>
        /// A test for GetAccessToken
        ///</summary>
        [Test]
        public void GetAccessTokenTest()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", "https://api.sandbox.paypal.com");
            string clientId = "EBWKjlELKMYqRNQ6sYvFo64FtaRLRR5BdHEESmha49TM";
            string clientSecret = "EO422dn3gQLgDbuwqTjzrFgFtaRLRR5BdHEESmha49TM";
            OAuthTokenCredential target = new OAuthTokenCredential(clientId, clientSecret, config);
            string expected = string.Empty;
            string actual;
            actual = target.GetAccessToken();
            Assert.AreEqual(true, actual.StartsWith("Bearer "));
        }

        /// <summary>
        /// A test for GetAccessToken
        /// </summary>
        [Test, ExpectedException(typeof(Exception.PayPalException))]
        public void GetAccessTokenInvalidEndpointTest()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", "https://localhost.sandbox.paypal.com");
            string clientId = "EBWKjlELKMYqRNQ6sYvFo64FtaRLRR5BdHEESmha49TM";
            string clientSecret = "EO422dn3gQLgDbuwqTjzrFgFtaRLRR5BdHEESmha49TM";
            OAuthTokenCredential target = new OAuthTokenCredential(clientId, clientSecret, config);
            string expected = string.Empty;
            string actual;
            actual = target.GetAccessToken();
            Assert.AreEqual(true, actual.StartsWith("Bearer "));
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PayPal.UnitTest
{  
    /// <summary>
    /// This is a test class for OAuthTokenCredentialTest and is intended
    /// to contain all OAuthTokenCredentialTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OAuthTokenCredentialTest
    {
        /// <summary>
        ///A test for GetAccessToken
        ///</summary>
        [TestMethod()]
        public void GetAccessTokenTest()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", "https://api.sandbox.paypal.com");
            string clientId = "EBWKjlELKMYqRNQ6sYvFo64FtaRLRR5BdHEESmha49TM"; 
            string clientSecret = "EO422dn3gQLgDbuwqTjzrFgFtaRLRR5BdHEESmha49TM";
            OAuthTokenCredential target = new OAuthTokenCredential(clientId, clientSecret, config);
            string expected = string.Empty;
            string actual;
            actual = target.GetAccessToken();
            Assert.AreEqual(true, actual.StartsWith("Bearer "));
        }

        /// <summary>
        /// A test for GetAccessToken
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.PayPalException))]
        public void GetAccessTokenInvalidEndpointTest()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", "https://localhost.sandbox.paypal.com");
            string clientId = "EBWKjlELKMYqRNQ6sYvFo64FtaRLRR5BdHEESmha49TM";
            string clientSecret = "EO422dn3gQLgDbuwqTjzrFgFtaRLRR5BdHEESmha49TM";
            OAuthTokenCredential target = new OAuthTokenCredential(clientId, clientSecret, config);
            string expected = string.Empty;
            string actual;
            actual = target.GetAccessToken();
            Assert.AreEqual(true, actual.StartsWith("Bearer "));
        }
    }
}

#endif

