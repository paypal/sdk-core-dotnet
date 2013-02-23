using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using PayPal.Exception;
using PayPal.Manager;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    public class ConnectionManagerTest : TestsBase
    {
        HttpWebRequest httpRequest;

        [Test]
        public void CreateNewConnection()
        {
            httpRequest = AppConnMgr.GetConnection("http://paypal.com/");
            Assert.IsNotNull(httpRequest);
            Assert.AreEqual("http://paypal.com/", httpRequest.RequestUri.AbsoluteUri);
            Assert.AreEqual(AppConfigMgr.GetProperty("connectionTimeout"), httpRequest.Timeout.ToString());
        }

        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateNewConnectionWithInvalidURL()
        {
            httpRequest = AppConnMgr.GetConnection("Not a url");
        }
    }
}
