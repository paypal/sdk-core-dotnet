using System.Collections.Generic;
using System.Net;
using PayPal.Manager;
using PayPal.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
{
    [TestClass]
    public class ConnectionManagerTest
    {
        private ConnectionManager connectionMngr;
        private HttpWebRequest httpRequest;

        [TestMethod]
        public void CreateNewConnection()
        {
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            connectionMngr = ConnectionManager.Instance;
            httpRequest = connectionMngr.GetConnection(config, "http://paypal.com/");
            Assert.IsNotNull(httpRequest);
            Assert.AreEqual("http://paypal.com/", httpRequest.RequestUri.AbsoluteUri);
            Assert.AreEqual(config["connectionTimeout"], httpRequest.Timeout.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException), "Invalid URI Not a url")]
        public void CreateNewConnectionWithInvalidUrl()
        {
            connectionMngr = ConnectionManager.Instance;
            httpRequest = connectionMngr.GetConnection(ConfigManager.Instance.GetProperties(), "Not a url");            
        }
    }
}