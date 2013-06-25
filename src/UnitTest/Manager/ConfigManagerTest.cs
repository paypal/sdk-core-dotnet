using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NUnit.Framework;
using PayPal.Manager;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    class ConfigManagerTest
    {
        ConfigManager configMngr;

        [Test]
        public void RetrieveValidProperty()
        {
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            string endpoint = config["endpoint"];
            Assert.IsNotNull(endpoint);
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
            string connectionTimeout = config["connectionTimeout"];
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("360000", connectionTimeout);
        }
    }
}
