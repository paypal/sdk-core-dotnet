using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal
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
        [ExpectedException(typeof(ConfigException))]
        public void CreateNewConnectionWithInvalidURL()
        {
            try
            {
                connectionMngr = ConnectionManager.Instance;
                httpRequest = connectionMngr.GetConnection(ConfigManager.Instance.GetProperties(), "Not a url");
            }
            catch (ConfigException ex)
            {
                Assert.AreEqual("Invalid URI Not a url", ex.Message);
                throw;
            }   
        }
    }
}
