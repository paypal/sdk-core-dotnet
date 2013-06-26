using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Manager;

namespace PayPal
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
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
            string connectionTimeout = config["connectionTimeout"];
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("360000", connectionTimeout);
        }
    }
}
