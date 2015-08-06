using System.Collections.Generic;
using PayPal.Authentication;
using PayPal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
{ 
    /// <summary>
    /// This is a test class for RESTAPICallPreHandlerTest and is intended
    /// to contain all RESTAPICallPreHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RESTAPICallPreHandlerTest
    {
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
        public void RESTAPICallPreHandlerPayloadTest()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("endpoint", "https://localhost.sandbox.paypal.com");
            RESTAPICallPreHandler target = new RESTAPICallPreHandler(config);
            target.Payload = "{ key : value}";
            Assert.AreEqual(target.Payload, "{ key : value}");
        }        
    }
}