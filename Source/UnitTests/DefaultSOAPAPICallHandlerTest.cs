using PayPal;
using PayPal.Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

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

    [TestMethod]
    [ExpectedException(typeof(System.ArgumentNullException), "System.ArgumentNullException: Value cannot be null.")]
    public void DomNullConfigMapTest()
    {
        APIContext api = new APIContext();
        Dictionary<string, string> configurationMap = null;
        new DefaultSOAPAPICallHandler(new SampleBody(), api, configurationMap, "DoDirectPayment");
    }

    [TestMethod]
    public void DomEndpointTest()
    {
        APIContext baseAPIContext = new APIContext();
        Dictionary<string, string> configurationMap = new Dictionary<string, string>();
        configurationMap.Add("endpoint", "https://api-3t.sandbox.paypal.com/2.0");
        DefaultSOAPAPICallHandler defHandler = new DefaultSOAPAPICallHandler(new SampleBody(), baseAPIContext, configurationMap, "DoDirectPayment");
        Assert.AreEqual("https://api-3t.sandbox.paypal.com/2.0", defHandler.GetEndpoint());
    }
    
    [TestMethod]
    public void DomPayloadTest()
    {
        DefaultSOAPAPICallHandler.XMLNamespaceProvider = new XmlNamespacePrefixProvider();        
        APIContext api = new APIContext();
        api.SOAPHeader = new SampleHeader();
        Dictionary<string, string> configurationMap = new Dictionary<string, string>();
        configurationMap.Add("service.EndPoint", "https://api-3t.sandbox.paypal.com/2.0");
        api.Config = configurationMap;

        DefaultSOAPAPICallHandler defHandler = new DefaultSOAPAPICallHandler(new SampleBody(), api, null, "DoDirectPayment");
        string payload = defHandler.GetPayload().Trim();
        string expectedPayload = "<soapenv:Envelope xmlns:xml=\"http://www.w3.org/XML/1998/namespace\" xmlns:ed=\"urn:ebay:apis:EnhancedDataTypes\" xmlns:cc=\"urn:ebay:apis:CoreComponentTypes\" xmlns:ebl=\"urn:ebay:apis:eBLBaseComponents\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:wsdlsoap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:ns=\"urn:ebay:api:PayPalAPI\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"  + 
            "<soapenv:Header>" +
            "<ns:RequesterCredentials>" +
            "<ebl:Credentials>" +
            "<ebl:Username>jb-us-seller_api1.paypal.com</ebl:Username>" +
            "</ebl:Credentials>" +
            "</ns:RequesterCredentials>" +
            "</soapenv:Header>" +
            "<soapenv:Body>" +
            "<ns:DoDirectPaymentReq>" +
            "<ns:DoDirectPaymentRequest>" +
            "<ebl:Version>98.0</ebl:Version>" +
            "<ebl:DoDirectPaymentRequestDetails>" +
            "<ebl:CreditCard>" +
            "<ebl:CreditCardType>Visa</ebl:CreditCardType>" +
            "<ebl:CreditCardNumber>4202297003827029</ebl:CreditCardNumber>" +
            "<ebl:CVV2>962</ebl:CVV2>" +
            "</ebl:CreditCard>" +
            "</ebl:DoDirectPaymentRequestDetails>" +
            "</ns:DoDirectPaymentRequest>" +
            "</ns:DoDirectPaymentReq>" +
            "</soapenv:Body>" +
        "</soapenv:Envelope>";
        Assert.AreEqual(expectedPayload, payload);
    }
    DefaultSOAPAPICallHandler defaultHandler;


    public void GetPayloadForEmptyRawPayloadTest()
    {
        defaultHandler = new DefaultSOAPAPICallHandler(new Dictionary<string, string>(), string.Empty, string.Empty, string.Empty);
        Assert.AreEqual("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" ><soapenv:Header></soapenv:Header><soapenv:Body></soapenv:Body></soapenv:Envelope>", defaultHandler.GetPayload());
    }

    private class XmlNamespacePrefixProvider : PayPal.DefaultSOAPAPICallHandler.XmlNamespaceProvider 
    {
        private Dictionary<string, string> namespaceMap;

		public XmlNamespacePrefixProvider() 
        {
            namespaceMap = new Dictionary<string, string>();
			namespaceMap.Add("xml", "http://www.w3.org/XML/1998/namespace");
			namespaceMap.Add("ed", "urn:ebay:apis:EnhancedDataTypes");
			namespaceMap.Add("cc", "urn:ebay:apis:CoreComponentTypes");
			namespaceMap.Add("ebl", "urn:ebay:apis:eBLBaseComponents");
			namespaceMap.Add("xs", "http://www.w3.org/2001/XMLSchema");
			namespaceMap.Add("wsdlsoap", "http://schemas.xmlsoap.org/wsdl/soap/");
			namespaceMap.Add("ns", "urn:ebay:api:PayPalAPI");
			namespaceMap.Add("SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/");
			namespaceMap.Add("wsdl", "http://schemas.xmlsoap.org/wsdl/");
		}

        public Dictionary<string, string> GetNamespaceDictionary() 
        {
			return namespaceMap;
		}
	}

	private class SampleHeader : XMLMessageSerializer 
    {
		public string ToXMLString() 
        {
			return "<ns:RequesterCredentials><ebl:Credentials><ebl:Username>jb-us-seller_api1.paypal.com</ebl:Username></ebl:Credentials></ns:RequesterCredentials>";
		}
	}

	private class SampleBody : XMLMessageSerializer 
    {
        public string ToXMLString()
        {
			return "<ns:DoDirectPaymentReq><ns:DoDirectPaymentRequest><ebl:Version>98.0</ebl:Version><ebl:DoDirectPaymentRequestDetails><ebl:CreditCard><ebl:CreditCardType>Visa</ebl:CreditCardType><ebl:CreditCardNumber>4202297003827029</ebl:CreditCardNumber><ebl:CVV2>962</ebl:CVV2></ebl:CreditCard></ebl:DoDirectPaymentRequestDetails></ns:DoDirectPaymentRequest></ns:DoDirectPaymentReq>";
		}
    }

	private class SampleNoNSBody : XMLMessageSerializer 
    {
        public string ToXMLString()
        {
			return "<ns:DoDirectPaymentReq><ns:DoDirectPaymentRequest><ebl:Version>98.0</ebl:Version><ebl:DoDirectPaymentRequestDetails><ebl:CreditCard><ebl:CreditCardType>Visa</ebl:CreditCardType><ebl:CreditCardNumber>4202297003827029</ebl:CreditCardNumber><ebl:CVV2>962</ebl:CVV2></ebl:CreditCard></ebl:DoDirectPaymentRequestDetails></ns:DoDirectPaymentRequest>";
		}	
    }
}