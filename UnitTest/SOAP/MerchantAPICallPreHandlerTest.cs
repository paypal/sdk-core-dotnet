using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.SOAP;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class MerchantAPICallPreHandlerTest : TestsBase
    {
        private DefaultSOAPAPICallHandler defaultSoapHandler;
        ICredential credential;
        MerchantAPICallPreHandler soapHandler;

        public MerchantAPICallPreHandlerTest()
        {
            defaultSoapHandler = new DefaultSOAPAPICallHandler(AppConfigMgr, "<Request>test</Request>", null, null);
        }

        [Test]
        public void GetHeaderMapSignature()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.APIUserName);
            soapHandler = new MerchantAPICallPreHandler(AppCredentialMgr, defaultSoapHandler, credential);
            Dictionary<String, String> headers = soapHandler.GetHeaderMap();
            Assert.IsNotNull(headers);
            Assert.IsTrue(headers.Count > 0);
            Assert.AreEqual(UnitTestConstants.APIUserName, headers[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual(UnitTestConstants.APIPassword, headers[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(UnitTestConstants.APISignature, headers[BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
        }

        [Test]
        public void GetHeaderMapCertificate()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.CertificateAPIUserName);
            soapHandler = new MerchantAPICallPreHandler(AppCredentialMgr, defaultSoapHandler, credential);
            Dictionary<String, String> headers = soapHandler.GetHeaderMap();
            Assert.IsNotNull(headers);
            Assert.IsTrue(headers.Count > 0);
            Assert.AreEqual(UnitTestConstants.CertificateAPIUserName, headers[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual(UnitTestConstants.CertificateAPIPassword, headers[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
            Assert.AreEqual(soapHandler.SDKName + "-" + soapHandler.SDKVersion, headers[BaseConstants.PAYPAL_REQUEST_SOURCE_HEADER]);
        }

        [Test]
        public void GetPayLoadSignature()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.APIUserName);
            soapHandler = new MerchantAPICallPreHandler(AppCredentialMgr, defaultSoapHandler, credential);
            string payload = soapHandler.GetPayLoad();
            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual(UnitTestConstants.APIUserName, xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual(UnitTestConstants.APIPassword, xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListSignature = xmlDoc.GetElementsByTagName("Signature");
            Assert.IsTrue(xmlNodeListSignature.Count > 0);
            Assert.AreEqual(UnitTestConstants.APISignature, xmlNodeListSignature[0].InnerXml);
            XmlNodeList xmlNodeListRequest = xmlDoc.GetElementsByTagName("Request");
            Assert.IsTrue(xmlNodeListRequest.Count > 0);
            Assert.AreEqual("test", xmlNodeListRequest[0].InnerXml);          
        }

        [Test]
        public void GetPayLoadForCertificate()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.CertificateAPIUserName);
            soapHandler = new MerchantAPICallPreHandler(AppCredentialMgr, defaultSoapHandler, credential);
            string payload = soapHandler.GetPayLoad();
            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual(UnitTestConstants.CertificateAPIUserName, xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual(UnitTestConstants.CertificateAPIPassword, xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListRequest = xmlDoc.GetElementsByTagName("Request");
            Assert.IsTrue(xmlNodeListRequest.Count > 0);
            Assert.AreEqual("test", xmlNodeListRequest[0].InnerXml);
        }
        
        [Test]
        public void SDKName()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(AppCredentialMgr, defaultSoapHandler, credential);
            soapHandler.SDKName = "testsdk";
            Assert.AreEqual("testsdk", soapHandler.SDKName);
        }

        [Test]
        public void SDKVersion()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(AppCredentialMgr, defaultSoapHandler, credential);
            soapHandler.SDKVersion = "1.0.0";
            Assert.AreEqual("1.0.0", soapHandler.SDKVersion);
        }

        [Test]
        public void GetEndPoint()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(AppConfigMgr, AppCredentialMgr, defaultSoapHandler, UnitTestConstants.APIUserName, null, null);
            string endpoint = soapHandler.GetEndPoint();
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
        }      

        private XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlString = xmlString.Replace("soapenv:", string.Empty);
            xmlString = xmlString.Replace("ebl:", string.Empty);
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }
    }
}
