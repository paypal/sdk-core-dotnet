using PayPal;
using PayPal.Manager;

#if NUnit
/* NuGet Install
 * Visual Studio 2005
    * Install NUnit -OutputDirectory .\packages
    * Add reference from NUnit.2.6.2
 */
using NUnit.Framework;

namespace PayPal.NUnitTest
{
    [TestFixture]
    class DefaultSOAPAPICallHandlerTest
    {
        DefaultSOAPAPICallHandler defaultSOAPHandler;

        // [Test] // <!--SOAP--> To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        [Ignore]
        public void Endpoint()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, string.Empty, string.Empty);
            Assert.AreEqual(Constants.APIEndpointSOAP, defaultSOAPHandler.GetEndpoint());
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
            Assert.AreEqual("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" ><soapenv:Header></soapenv:Header><soapenv:Body></soapenv:Body></soapenv:Envelope>", defaultSOAPHandler.GetPayload());
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class DefaultSOAPAPICallHandlerTest
{
    DefaultSOAPAPICallHandler defaultSOAPHandler;

    // [TestMethod] // <!--SOAP--> To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
    [Ignore] 
    public void Endpoint() 
    {
        defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, string.Empty, string.Empty);
	    Assert.AreEqual(Constants.APIEndpointSOAP, defaultSOAPHandler.GetEndpoint());
    }

    [TestMethod]
    public void HeaderElement()
    {
        defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), string.Empty, string.Empty, string.Empty);
        defaultSOAPHandler.HeaderElement = "HeaderElement";
        Assert.AreEqual("HeaderElement", defaultSOAPHandler.HeaderElement);
    }

    [TestMethod]
    public void NamespaceAttributes()
    {
        defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), string.Empty, string.Empty, string.Empty);
        defaultSOAPHandler.NamespaceAttributes = "NamespaceAttributes";
        Assert.AreEqual("NamespaceAttributes", defaultSOAPHandler.NamespaceAttributes);
    }

    [TestMethod]
    public void GetPayloadForEmptyRawPayload()
    {
        defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), string.Empty, string.Empty, string.Empty);
        Assert.AreEqual("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" ><soapenv:Header></soapenv:Header><soapenv:Body></soapenv:Body></soapenv:Envelope>", defaultSOAPHandler.GetPayload());
    }
}
#endif
