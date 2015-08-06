using System;
using System.Collections.Generic;
using PayPal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
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
            string accessToken = this.GetAccessToken(OAuthTokenCredentialTest.endpoint, OAuthTokenCredentialTest.clientId, OAuthTokenCredentialTest.clientSecret);
            Assert.AreEqual(true, accessToken.StartsWith("Bearer "));
        }

        /// <summary>
        /// A test for GetAccessToken
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.ConnectionException))]
        public void GetAccessTokenInvalidEndpointTest()
        {
            string accessToken = this.GetAccessToken("https://localhost.sandbox.paypal.com", OAuthTokenCredentialTest.clientId, OAuthTokenCredentialTest.clientSecret);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.IdentityException))]
        public void GetAccessTokenInvalidClientId()
        {
            string accessToken = this.GetAccessToken(OAuthTokenCredentialTest.endpoint, "invalid_client_id", OAuthTokenCredentialTest.clientSecret);
        }

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.IdentityException))]
        public void GetAccessTokenInvalidClientSecret()
        {
            string accessToken = this.GetAccessToken(OAuthTokenCredentialTest.endpoint, OAuthTokenCredentialTest.clientId, "invalid_client_secret");
        }

#if !NUnit
        [TestMethod()]
        public void Verify64BitEncodingWithValidCredentials()
        {
            string credentials = this.ConvertClientCredentialsToBase64String(OAuthTokenCredentialTest.clientId, OAuthTokenCredentialTest.clientSecret);
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
#endif

        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.ConnectionException))]
        public void GetAccessTokenTimeoutTest()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config[BaseConstants.ApplicationModeConfig] = BaseConstants.SandboxMode;
            config[BaseConstants.HttpConnectionTimeoutConfig] = "10";
            OAuthTokenCredential target = new OAuthTokenCredential(clientId, clientSecret, config);
            string accessToken = target.GetAccessToken();
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
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", endpoint);
            OAuthTokenCredential target = new OAuthTokenCredential(clientId, clientSecret, config);
            return target.GetAccessToken();
        }

#if !NUnit
        /// <summary>
        /// Helper method for calling <see cref="OAuthTokenCredentia.ConvertClientCredentialsToBase64String"/> from any unit test.
        /// </summary>
        /// <param name="clientId">The clientId to use in generating the credentials base-64 string.</param>
        /// <param name="clientSecret">The clientSecret to use in generating the credentials base-64 string</param>
        /// <returns>A base-64 encoded string containing the client credentials.</returns>
        private string ConvertClientCredentialsToBase64String(string clientId, string clientSecret)
        {
            PrivateType oauthTokenCredential = new PrivateType(typeof(OAuthTokenCredential));
            return oauthTokenCredential.InvokeStatic("ConvertClientCredentialsToBase64String", new string[] { clientId, clientSecret }) as string;
        }
#endif
    }
}