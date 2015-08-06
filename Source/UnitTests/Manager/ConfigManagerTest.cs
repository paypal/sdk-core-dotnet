using System.Collections.Generic;
using PayPal.Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
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