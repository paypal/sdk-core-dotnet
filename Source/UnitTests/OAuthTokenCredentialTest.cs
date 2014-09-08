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
        private static readonly string endpoint = "https://api.sandbox.paypal.com";
        private static readonly string clientId = "EBWKjlELKMYqRNQ6sYvFo64FtaRLRR5BdHEESmha49TM";
        private static readonly string clientSecret = "EO422dn3gQLgDbuwqTjzrFgFtaRLRR5BdHEESmha49TM";

        /// <summary>
        ///A test for GetAccessToken
        ///</summary>
        [TestMethod()]
        public void GetAccessTokenTest()
        {
            var accessToken = this.GetAccessToken(OAuthTokenCredentialTest.endpoint, OAuthTokenCredentialTest.clientId, OAuthTokenCredentialTest.clientSecret);
            Assert.AreEqual(true, accessToken.StartsWith("Bearer "));
        }

        /// <summary>
        /// A test for GetAccessToken
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.PayPalException))]
        public void GetAccessTokenInvalidEndpointTest()
        {
            var accessToken = this.GetAccessToken("https://localhost.sandbox.paypal.com", OAuthTokenCredentialTest.clientId, OAuthTokenCredentialTest.clientSecret);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.IdentityException))]
        public void GetAccessTokenInvalidClientId()
        {
            var accessToken = this.GetAccessToken(OAuthTokenCredentialTest.endpoint, "invalid_client_id", OAuthTokenCredentialTest.clientSecret);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.IdentityException))]
        public void GetAccessTokenInvalidClientSecret()
        {
            var accessToken = this.GetAccessToken(OAuthTokenCredentialTest.endpoint, OAuthTokenCredentialTest.clientId, "invalid_client_secret");
        }

        [TestMethod()]
        public void Verify64BitEncodingWithValidCredentials()
        {
            var credentials = this.ConvertClientCredentialsToBase64String(OAuthTokenCredentialTest.clientId, OAuthTokenCredentialTest.clientSecret);
            Assert.AreEqual("RUJXS2psRUxLTVlxUk5RNnNZdkZvNjRGdGFSTFJSNUJkSEVFU21oYTQ5VE06RU80MjJkbjNnUUxnRGJ1d3FUanpyRmdGdGFSTFJSNUJkSEVFU21oYTQ5VE0=", credentials);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.MissingCredentialException))]
        public void Verify64BitEncodingWithNullClientId()
        {
            this.ConvertClientCredentialsToBase64String(null, OAuthTokenCredentialTest.clientSecret);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.MissingCredentialException))]
        public void Verify64BitEncodingWithNullClientSecret()
        {
            this.ConvertClientCredentialsToBase64String(OAuthTokenCredentialTest.clientId, null);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.MissingCredentialException))]
        public void Verify64BitEncodingWithEmptyClientId()
        {
            this.ConvertClientCredentialsToBase64String("", OAuthTokenCredentialTest.clientSecret);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.MissingCredentialException))]
        public void Verify64BitEncodingWithEmptyClientSecret()
        {
            this.ConvertClientCredentialsToBase64String(OAuthTokenCredentialTest.clientId, "");
        }

        /// <summary>
        /// Helper method for getting an access token for test purposes.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        private string GetAccessToken(string endpoint, string clientId, string clientSecret)
        {
            var config = new Dictionary<string, string>();
            config.Add("endpoint", endpoint);
            var target = new OAuthTokenCredential(clientId, clientSecret, config);
            return target.GetAccessToken();
        }

        /// <summary>
        /// Helper method for calling <see cref="OAuthTokenCredentia.ConvertClientCredentialsToBase64String"/> from any unit test.
        /// </summary>
        /// <param name="clientId">The clientId to use in generating the credentials base-64 string.</param>
        /// <param name="clientSecret">The clientSecret to use in generating the credentials base-64 string</param>
        /// <returns>A base-64 encoded string containing the client credentials.</returns>
        private string ConvertClientCredentialsToBase64String(string clientId, string clientSecret)
        {
            var oauthTokenCredential = new PrivateType(typeof(OAuthTokenCredential));
            return oauthTokenCredential.InvokeStatic("ConvertClientCredentialsToBase64String", new string[] { clientId, clientSecret }) as string;
        }
    }
}

#endif

