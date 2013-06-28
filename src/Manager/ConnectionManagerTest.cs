using System.Collections.Generic;
using System.Net;
using PayPal.Manager;
using PayPal.Exception;

#if NUnit
using NUnit.Framework;

namespace PayPal.NUnitTest
{
    [TestFixture]
    public class ConnectionManagerTest
    {
        ConnectionManager connectionMngr;
        HttpWebRequest httpRequest;

        [Test]
        public void CreateNewConnection()
        {
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            connectionMngr = ConnectionManager.Instance;
            httpRequest = connectionMngr.GetConnection(config, "http://paypal.com/");
            Assert.IsNotNull(httpRequest);
            Assert.AreEqual("http://paypal.com/", httpRequest.RequestUri.AbsoluteUri);
            Assert.AreEqual(config["connectionTimeout"], httpRequest.Timeout.ToString());
        }

        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateNewConnectionWithInvalidURL()
        {
            connectionMngr = ConnectionManager.Instance;
            httpRequest = connectionMngr.GetConnection(ConfigManager.Instance.GetProperties(), "Not a url");
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
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
#endif
