using PayPal;

using System;
using System.Collections.Generic;
using PayPal.Authentication;

#if NUnit

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PayPal.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for RESTAPICallPreHandlerTest and is intended
    ///to contain all RESTAPICallPreHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RESTAPICallPreHandlerTest
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
        ///A test for RESTAPICallPreHandler
        ///</summary>
        [TestMethod()]
        public void RESTAPICallPreHandlerEndpointTest()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", "https://localhost.sandbox.paypal.com");
            RESTAPICallPreHandler target = new RESTAPICallPreHandler(config);
            Assert.AreEqual(target.GetEndpoint().EndsWith("/"), true);
        }

        /// <summary>
        ///A test for RESTAPICallPreHandler Constructor
        ///</summary>
        [TestMethod()]
        public void RESTAPICallPreHandlerPayloadTest1()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", "https://localhost.sandbox.paypal.com");
            RESTAPICallPreHandler target = new RESTAPICallPreHandler(config);
            target.Payload = "{ key : value}";
            Assert.AreEqual(target.Payload, "{ key : value}");
        }

        
    }
}
#endif

