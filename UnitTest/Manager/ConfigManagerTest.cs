using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NUnit.Framework;
using PayPal.Manager;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    class ConfigManagerTest : TestsBase
    {
        [Test]
        public void RetrieveAccountConfigByIndex()
        {
            Account acc = ConfigMgr.GetAccount(0);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
        }

        [Test]
        public void RetrieveAccountConfigByUsername()
        {
            Account acc = ConfigMgr.GetAccount(UnitTestConstants.APIUserName);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
            Assert.AreEqual(UnitTestConstants.APIPassword, acc.APIPassword);
            Assert.AreEqual(UnitTestConstants.APISignature, acc.APISignature);
            Assert.AreEqual(UnitTestConstants.ApplicationID, acc.ApplicationId);
        }

        [Test]
        public void RetrieveNonExistentAccount()
        {
            Account acc = ConfigMgr.GetAccount("i-do-not-exist_api1.paypal.com");
            Assert.IsNull(acc, "Invalid account name returns null account config");
        }

        [Test]
        public void RetrieveValidProperty()
        {
            string endpoint = ConfigMgr.GetProperty("endpoint");
            Assert.IsNotNull(endpoint);
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
            string connectionTimeout = ConfigMgr.GetProperty("connectionTimeout");
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("3600000", connectionTimeout);
        }

        [Test]
        public void RetrieveNonExistentProperty()
        {
            string endpoint = ConfigMgr.GetProperty("endpointMisspelt");
            Assert.IsNull(endpoint);
        }

    }
}
