using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.Exception;
using PayPal.Authentication;
using PayPal.SOAP;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class MerchantAPICallPreHandlerTest
    {
        private DefaultSOAPAPICallHandler defaultSoapHandler;
        private CredentialManager credentialMngr;
        private ICredential credential;
        private MerchantAPICallPreHandler soapHandler;
        private Dictionary<string, string> accountConfig;

        public MerchantAPICallPreHandlerTest()
        {
            defaultSoapHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), "<Request>test</Request>", null, null);
            credentialMngr = CredentialManager.Instance;

            accountConfig = new Dictionary<string, string>();
            accountConfig.Add("account1.apiUsername", UnitTestConstants.APIUserName);
            accountConfig.Add("account1.apiPassword", UnitTestConstants.APIPassword);
            accountConfig.Add("account1.applicationId", UnitTestConstants.ApplicationID);
            accountConfig.Add("account1.apiSignature", UnitTestConstants.APISignature);
            accountConfig.Add("account2.apiUsername", UnitTestConstants.CertificateAPIUserName);
            accountConfig.Add("account2.apiPassword", UnitTestConstants.CertificateAPIPassword);
            accountConfig.Add("account2.applicationId", UnitTestConstants.ApplicationID);
            accountConfig.Add("account2.apiCertificate", UnitTestConstants.CertificatePath);
            accountConfig.Add("account2.privateKeyPassword", UnitTestConstants.CertificatePassword);
        }

        [Test]
        public void GetHeaderMapSignature()
        {
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.APIUserName);
            soapHandler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSoapHandler, credential);
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
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.CertificateAPIUserName);
            soapHandler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSoapHandler, credential);
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
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.APIUserName);
            soapHandler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSoapHandler, credential);
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
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.CertificateAPIUserName);
            soapHandler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSoapHandler, credential);
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
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSoapHandler, credential);
            soapHandler.SDKName = "testsdk";
            Assert.AreEqual("testsdk", soapHandler.SDKName);
        }

        [Test]
        public void SDKVersion()
        {
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSoapHandler, credential);
            soapHandler.SDKVersion = "1.0.0";
            Assert.AreEqual("1.0.0", soapHandler.SDKVersion);
        }

        [Test]
        public void GetEndPoint()
        {
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSoapHandler, credential);
            string endpoint = soapHandler.GetEndPoint();
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
        }

        [Test]
        public void GetEndpointForSandboxMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.APPLICATION_MODE_CONFIG, BaseConstants.LIVE_MODE);
            
            credential = credentialMngr.GetCredentials(config, UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(config, defaultSoapHandler, credential);
            Assert.AreEqual(BaseConstants.MERCHANT_CERTIFICATE_LIVE_ENDPOINT, soapHandler.GetEndPoint());

            credential = credentialMngr.GetCredentials(config, UnitTestConstants.APIUserName);
            soapHandler = new MerchantAPICallPreHandler(config, defaultSoapHandler, credential);
            Assert.AreEqual(BaseConstants.MERCHANT_SIGNATURE_LIVE_ENDPOINT, soapHandler.GetEndPoint());
        }

        [Test]
        public void GetEndpointForLiveMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.APPLICATION_MODE_CONFIG, BaseConstants.SANDBOX_MODE);

            credential = credentialMngr.GetCredentials(config, UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(config, defaultSoapHandler, credential);
            Assert.AreEqual(BaseConstants.MERCHANT_CERTIFICATE_SANDBOX_ENDPOINT, soapHandler.GetEndPoint());

            credential = credentialMngr.GetCredentials(config, UnitTestConstants.APIUserName);
            soapHandler = new MerchantAPICallPreHandler(config, defaultSoapHandler, credential);
            Assert.AreEqual(BaseConstants.MERCHANT_SIGNATURE_SANDBOX_ENDPOINT, soapHandler.GetEndPoint());
        }

        [ExpectedException(typeof(ConfigException))]
        [Test]
        public void GetEndpointForDefaultModeWithoutEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);

            credential = credentialMngr.GetCredentials(config, UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(config, defaultSoapHandler, credential);
            soapHandler.GetEndPoint();
        }

        [Test]
        public void GetEndpointForDefaultModeWithExplicitEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.END_POINT_CONFIG, UnitTestConstants.APIEndpointNVP);

            credential = credentialMngr.GetCredentials(config, UnitTestConstants.CertificateAPIUserName);
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(config, defaultSoapHandler, credential);
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, soapHandler.GetEndPoint());


            config.Add("PayPalAPI", UnitTestConstants.APIEndpointSOAP);
            credential = credentialMngr.GetCredentials(config, UnitTestConstants.CertificateAPIUserName);
            soapHandler = new MerchantAPICallPreHandler(config, defaultSoapHandler, credential);
            soapHandler.PortName = "PayPalAPI";
            Assert.AreEqual(UnitTestConstants.APIEndpointSOAP, soapHandler.GetEndPoint());
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
