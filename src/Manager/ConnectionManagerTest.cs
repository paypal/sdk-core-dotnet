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
        private ConnectionManager ConnectionMngr;
        private HttpWebRequest HttpRequest;

        [TestMethod]
        public void CreateNewConnection()
        {
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            ConnectionMngr = ConnectionManager.Instance;
            HttpRequest = ConnectionMngr.GetConnection(config, "http://paypal.com/");
            Assert.IsNotNull(HttpRequest);
            Assert.AreEqual("http://paypal.com/", HttpRequest.RequestUri.AbsoluteUri);
            Assert.AreEqual(config["connectionTimeout"], HttpRequest.Timeout.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void CreateNewConnectionWithInvalidURL()
        {
            try
            {
                ConnectionMngr = ConnectionManager.Instance;
                HttpRequest = ConnectionMngr.GetConnection(ConfigManager.Instance.GetProperties(), "Not a url");
            }
            catch (ConfigException ex)
            {
                Assert.AreEqual("Invalid URI Not a url", ex.Message);
                throw;
            }   
        }
    }
}
