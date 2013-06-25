using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using PayPal.Exception;
using PayPal.Manager;

namespace PayPal.UnitTest.Manager
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
