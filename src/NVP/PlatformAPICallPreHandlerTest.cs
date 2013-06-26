using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Manager;
using PayPal.Exception;
using PayPal.Authentication;
using PayPal.NVP;

namespace PayPal
{
    [TestClass]
    public class PlatformAPICallPreHandlerTest
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

        [TestMethod]
        public void GetHeaderMapWithSignatureWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            string authHeader = header["X-PAYPAL-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [TestMethod]
        public void GetHeaderMapSignatureWithoutTokenTest()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.APIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", credential);            
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(UnitTestConstants.APIUserName, header[BaseConstants.PayPalSecurityUserIDHeader]);
            Assert.AreEqual(UnitTestConstants.APIPassword, header[BaseConstants.PayPalSecurityPasswordHeader]);
            Assert.AreEqual(UnitTestConstants.APISignature, header[BaseConstants.PayPalSecuritySignatureHeader]);
            Assert.AreEqual(UnitTestConstants.ApplicationID, header[BaseConstants.PayPalApplicationIDHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalRequestDataFormatHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalResponseDataFormatHeader]);
        }

        [TestMethod]
        public void GetHeaderMapWithCertificateWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", UnitTestConstants.CertificateAPIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();            
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [TestMethod]
        public void GetHeaderMapCertificateWithoutTokenTest()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), UnitTestConstants.CertificateAPIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", credential);
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(UnitTestConstants.CertificateAPIUserName, header[BaseConstants.PayPalSecurityUserIDHeader]);
            Assert.AreEqual(UnitTestConstants.CertificateAPIPassword, header[BaseConstants.PayPalSecurityPasswordHeader]);
            Assert.AreEqual(UnitTestConstants.ApplicationID, header[BaseConstants.PayPalApplicationIDHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalRequestDataFormatHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalResponseDataFormatHeader]);
        }  
                
        [TestMethod]
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

        [TestMethod]
        public void GetEndpointForSandboxMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.ApplicationModeConfig, BaseConstants.LiveMode);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(BaseConstants.PlatformLiveEndPoint + "servicename/method", platformHandler.GetEndPoint());
        }

        [TestMethod]
        public void GetEndpointForLiveMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.ApplicationModeConfig, BaseConstants.SandboxMode);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(BaseConstants.PlatformSandboxEndpoint + "servicename/method", platformHandler.GetEndPoint());

        }

        [ExpectedException(typeof(ConfigException))]
        [TestMethod]
        public void GetEndpointForDefaultModeWithoutEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            platformHandler.GetEndPoint();
        }

        [TestMethod]
        public void GetEndpointForDefaultModeWithExplicitEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.EndpointConfig, UnitTestConstants.APIEndpointNVP);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP + "servicename/method", platformHandler.GetEndPoint());


            config.Add("PayPalAPI", UnitTestConstants.APIEndpointSOAP);
            platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            platformHandler.PortName = "PayPalAPI";
            Assert.AreEqual(UnitTestConstants.APIEndpointSOAP + "/servicename/method", platformHandler.GetEndPoint());
        }

    }
}
