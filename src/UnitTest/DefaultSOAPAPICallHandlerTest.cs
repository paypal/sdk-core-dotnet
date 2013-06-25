using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PayPal.Manager;

namespace PayPal.UnitTest
{
    [TestFixture]
    class DefaultSOAPAPICallHandlerTest
    {
        DefaultSOAPAPICallHandler defaultSOAPHandler;

        // [Test] // <!--SOAP--> To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        [Ignore] 
	    public void EndPoint() 
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), UnitTestConstants.PayloadNVP, string.Empty, string.Empty);
		    Assert.AreEqual(UnitTestConstants.APIEndpointSOAP, defaultSOAPHandler.GetEndPoint());
        }
    
        [Test]
        public void HeaderElement()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), string.Empty, string.Empty, string.Empty);
            defaultSOAPHandler.HeaderElement = "HeaderElement";
            Assert.AreEqual("HeaderElement", defaultSOAPHandler.HeaderElement);
        }

        [Test]
        public void NamespaceAttributes()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), string.Empty, string.Empty, string.Empty);
            defaultSOAPHandler.NamespaceAttributes = "NamespaceAttributes";
            Assert.AreEqual("NamespaceAttributes", defaultSOAPHandler.NamespaceAttributes);
        }

        [Test]
        public void GetPayloadForEmptyRawPayload()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), string.Empty, string.Empty, string.Empty);
            Assert.AreEqual("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" ><soapenv:Header></soapenv:Header><soapenv:Body></soapenv:Body></soapenv:Envelope>", defaultSOAPHandler.GetPayLoad());
        }
    }
}
