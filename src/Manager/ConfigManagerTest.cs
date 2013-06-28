using System.Collections.Generic;
using PayPal.Manager;

#if NUnit
using NUnit.Framework;

namespace PayPal.NUnitTest
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
            Assert.AreEqual(Constants.APIEndpointNVP, endpoint);
            string connectionTimeout = config["connectionTimeout"];
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("360000", connectionTimeout);
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
{
    [TestClass]
    public class ConfigManagerTest
    {
        [TestMethod]
        public void RetrieveValidProperty()
        {
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            string endpoint = config["endpoint"];
            Assert.IsNotNull(endpoint);
            Assert.AreEqual(Constants.APIEndpointNVP, endpoint);
            string connectionTimeout = config["connectionTimeout"];
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("360000", connectionTimeout);
        }
    }
}
#endif
