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
            accountConfig.Add("account1.apiUsername", Constants.APIUserName);
            accountConfig.Add("account1.apiPassword", Constants.APIPassword);
            accountConfig.Add("account1.applicationId", Constants.ApplicationID);
            accountConfig.Add("account1.apiSignature", Constants.APISignature);
            accountConfig.Add("account2.apiUsername", Constants.CertificateAPIUserName);
            accountConfig.Add("account2.apiPassword", Constants.CertificateAPIPassword);
            accountConfig.Add("account2.applicationId", Constants.ApplicationID);
            accountConfig.Add("account2.apiCertificate", Constants.CertificatePath);
            accountConfig.Add("account2.privateKeyPassword", Constants.CertificatePassword);
        }

        [TestMethod]
        public void GetHeaderMapWithSignatureWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", Constants.APIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            string authHeader = header["X-PAYPAL-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [TestMethod]
        public void GetHeaderMapSignatureWithoutTokenTest()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), Constants.APIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", credential);            
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(Constants.APIUserName, header[BaseConstants.PayPalSecurityUserIDHeader]);
            Assert.AreEqual(Constants.APIPassword, header[BaseConstants.PayPalSecurityPasswordHeader]);
            Assert.AreEqual(Constants.APISignature, header[BaseConstants.PayPalSecuritySignatureHeader]);
            Assert.AreEqual(Constants.ApplicationID, header[BaseConstants.PayPalApplicationIDHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalRequestDataFormatHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalResponseDataFormatHeader]);
        }

        [TestMethod]
        public void GetHeaderMapWithCertificateWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", Constants.CertificateAPIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();            
            string authHeader = header[BaseConstants.PayPalAuthorizationPlatformHeader];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [TestMethod]
        public void GetHeaderMapCertificateWithoutTokenTest()
        {
            credentialMngr = CredentialManager.Instance;
            credential = credentialMngr.GetCredentials(ConfigManager.Instance.GetProperties(), Constants.CertificateAPIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", credential);
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(Constants.CertificateAPIUserName, header[BaseConstants.PayPalSecurityUserIDHeader]);
            Assert.AreEqual(Constants.CertificateAPIPassword, header[BaseConstants.PayPalSecurityPasswordHeader]);
            Assert.AreEqual(Constants.ApplicationID, header[BaseConstants.PayPalApplicationIDHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalRequestDataFormatHeader]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PayPalResponseDataFormatHeader]);
        }  
                
        [TestMethod]
        public void GetPayloadEndpointWithoutTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), "payload", "servicename", "method", Constants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual("https://svcs.sandbox.paypal.com/servicename/method", platformAPIHandler.GetEndPoint());
            Assert.AreEqual("payload", platformAPIHandler.GetPayLoad());
            SignatureCredential signatureCredential = (SignatureCredential)platformAPIHandler.GetCredential();
            TokenAuthorization thirdAuth = (TokenAuthorization)signatureCredential.ThirdPartyAuthorization;
            Assert.AreEqual("accessToken", thirdAuth.AccessToken);
            Assert.AreEqual("tokenSecret", thirdAuth.AccessTokenSecret);
        }

        [TestMethod]
        public void GetEndpointForSandboxMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.ApplicationModeConfig, BaseConstants.LiveMode);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", Constants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(BaseConstants.PlatformLiveEndPoint + "servicename/method", platformHandler.GetEndPoint());
        }

        [TestMethod]
        public void GetEndpointForLiveMode()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.ApplicationModeConfig, BaseConstants.SandboxMode);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", Constants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(BaseConstants.PlatformSandboxEndpoint + "servicename/method", platformHandler.GetEndPoint());

        }

        [ExpectedException(typeof(ConfigException))]
        [TestMethod]
        public void GetEndpointForDefaultModeWithoutEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", Constants.APIUserName, "accessToken", "tokenSecret");
            platformHandler.GetEndPoint();
        }

        [TestMethod]
        public void GetEndpointForDefaultModeWithExplicitEndpoint()
        {
            Dictionary<string, string> config = new Dictionary<string, string>(accountConfig);
            config.Add(BaseConstants.EndpointConfig, Constants.APIEndpointNVP);

            PlatformAPICallPreHandler platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", Constants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual(Constants.APIEndpointNVP + "servicename/method", platformHandler.GetEndPoint());


            config.Add("PayPalAPI", Constants.APIEndpointSOAP);
            platformHandler = new PlatformAPICallPreHandler(config, "payload", "servicename", "method", Constants.APIUserName, "accessToken", "tokenSecret");
            platformHandler.PortName = "PayPalAPI";
            Assert.AreEqual(Constants.APIEndpointSOAP + "/servicename/method", platformHandler.GetEndPoint());
        }

    }
}
