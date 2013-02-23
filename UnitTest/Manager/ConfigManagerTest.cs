using NUnit.Framework;
using PayPal.Manager;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    class ConfigManagerTest : TestsBase
    {
        #region AppConfigManagerTests

        [Test]
        public void RetrieveAppeAccountConfigByIndex()
        {
            IAccount acc = AppConfigMgr.GetAccount(0);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
        }

        [Test]
        public void RetrieveAppAccountConfigByUsername()
        {
            IAccount acc = AppConfigMgr.GetAccount(UnitTestConstants.APIUserName);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
            Assert.AreEqual(UnitTestConstants.APIPassword, acc.APIPassword);
            Assert.AreEqual(UnitTestConstants.APISignature, acc.APISignature);
            Assert.AreEqual(UnitTestConstants.ApplicationID, acc.ApplicationId);
        }

        [Test]
        public void RetrieveNonExistentAppAccount()
        {
            IAccount acc = AppConfigMgr.GetAccount("i-do-not-exist_api1.paypal.com");
            Assert.IsNull(acc, "Invalid account name returns null account config");
        }

        [Test]
        public void RetrieveValidAppProperty()
        {
            string endpoint = AppConfigMgr.GetProperty("endpoint");
            Assert.IsNotNull(endpoint);
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
            string connectionTimeout = AppConfigMgr.GetProperty("connectionTimeout");
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("3600000", connectionTimeout);
        }

        [Test]
        public void RetrieveNonExistentAppProperty()
        {
            string endpoint = AppConfigMgr.GetProperty("endpointMisspelt");
            Assert.IsNull(endpoint);
        }

        #endregion

        #region HashtableConfigManagerTests

        [Test]
        public void RetrieveHashtableAccountConfigByIndex()
        {
            IAccount acc = HashtableConfigMgr.GetAccount(0);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
        }

        [Test]
        public void RetrieveHashtableAccountConfigByUsername()
        {
            IAccount acc = HashtableConfigMgr.GetAccount(UnitTestConstants.APIUserName);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
            Assert.AreEqual(UnitTestConstants.APIPassword, acc.APIPassword);
            Assert.AreEqual(UnitTestConstants.APISignature, acc.APISignature);
            Assert.AreEqual(UnitTestConstants.ApplicationID, acc.ApplicationId);
        }

        [Test]
        public void RetrieveNonExistentHashtableAccount()
        {
            IAccount acc = HashtableConfigMgr.GetAccount("i-do-not-exist_api1.paypal.com");
            Assert.IsNull(acc, "Invalid account name returns null account config");
        }

        [Test]
        public void RetrieveValidHashtableProperty()
        {
            string endpoint = HashtableConfigMgr.GetProperty("endpoint");
            Assert.IsNotNull(endpoint);
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
            string connectionTimeout = HashtableConfigMgr.GetProperty("connectionTimeout");
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("3600000", connectionTimeout);
        }

        [Test]
        public void RetrieveNonExistentHashtableProperty()
        {
            string endpoint = HashtableConfigMgr.GetProperty("endpointMisspelt");
            Assert.IsNull(endpoint);
        }

        #endregion
    }
}
