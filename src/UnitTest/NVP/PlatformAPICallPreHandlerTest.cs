using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.Exception;
using PayPal.Authentication;
using PayPal.NVP;

namespace PayPal.UnitTest.NVP
{
    [TestFixture]
    class PlatformAPICallPreHandlerTest
    {
        private PlatformAPICallPreHandler platformAPIHandler;
        private CredentialManager credentialMngr;
        private ICredential credential;
        private Dictionary<string, string> accountConfig;

        public PlatformAPICallPreHandlerTest()
        {
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
        public void GetHeaderMapWithSignatureWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            string authHeader = header["X-PAYPAL-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void GetHeaderMapSignatureWithoutTokenTest()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.APIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", credential);            
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(UnitTestConstants.APIUserName, header[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual(UnitTestConstants.APIPassword, header[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(UnitTestConstants.APISignature, header[BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER]);
            Assert.AreEqual(UnitTestConstants.ApplicationID, header[BaseConstants.PAYPAL_APPLICATION_ID_HEADER]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
        }

        [Test]
        public void GetHeaderMapWithCertificateWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", UnitTestConstants.CertificateAPIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();            
            string authHeader = header[BaseConstants.PAYPAL_AUTHORIZATION_PLATFORM_HEADER];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void GetHeaderMapCertificateWithoutTokenTest()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.CertificateAPIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", credential);
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(UnitTestConstants.CertificateAPIUserName, header[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual(UnitTestConstants.CertificateAPIPassword, header[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(UnitTestConstants.ApplicationID, header[BaseConstants.PAYPAL_APPLICATION_ID_HEADER]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
        }  
                
        [Test]
        public void GetPayloadEndpointWithoutTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual("https://svcs.sandbox.paypal.com/servicename/method", platformAPIHandler.GetEndPoint());
            Assert.AreEqual("payload", platformAPIHandler.GetPayLoad());
            SignatureCredential signatureCredential = (SignatureCredential)platformAPIHandler.GetCredential();
            TokenAuthorization thirdAuth = (TokenAuthorization)signatureCredential.ThirdPartyAuthorization;
            Assert.AreEqual("accessToken", thirdAuth.AccessToken);
            Assert.AreEqual("tokenSecret", thirdAuth.TokenSecret);
        }

        [Test]
        public void GetEndpointForSandboxMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.APPLICATION_MODE_CONFIG, BaseConstants.LIVE_MODE);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(BaseConstants.PLATFORM_LIVE_ENDPOINT + "servicename/method", platformHandler.GetEndPoint());
        }

        [Test]
        public void GetEndpointForLiveMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.APPLICATION_MODE_CONFIG, BaseConstants.SANDBOX_MODE);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(BaseConstants.PLATFORM_SANDBOX_ENDPOINT + "servicename/method", platformHandler.GetEndPoint());

        }

        [ExpectedException(typeof(ConfigException))]
        [Test]
        public void GetEndpointForDefaultModeWithoutEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            platformHandler.GetEndPoint();
        }

        [Test]
        public void GetEndpointForDefaultModeWithExplicitEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.END_POINT_CONFIG, UnitTestConstants.APIEndpointNVP);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP + "servicename/method", platformHandler.GetEndPoint());


            config.Add("PayPalAPI", UnitTestConstants.APIEndpointSOAP);
            platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            platformHandler.PortName = "PayPalAPI";
            Assert.AreEqual(UnitTestConstants.APIEndpointSOAP + "/servicename/method", platformHandler.GetEndPoint());
        }

    }
}
