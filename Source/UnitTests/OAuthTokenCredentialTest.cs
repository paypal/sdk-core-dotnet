using PayPal;
using System;
using System.Collections.Generic;

#if NUnit

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PayPal.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for OAuthTokenCredentialTest and is intended
    ///to contain all OAuthTokenCredentialTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OAuthTokenCredentialTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

#region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


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

