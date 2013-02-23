using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.NVP;

namespace PayPal.UnitTest.NVP
{
    [TestFixture]
    class PlatformAPICallPreHandlerTest : TestsBase
    {
        PlatformAPICallPreHandler platformAPIHandler;
        ICredential credential;

        [Test]
        public void GetHeaderMapWithSignatureWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(AppConfigMgr, AppCredentialMgr, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            string authHeader = header["X-PAYPAL-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void GetHeaderMapSignatureWithoutTokenTest()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.APIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(AppConfigMgr, AppCredentialMgr, "payload", "servicename", "method", credential);            
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(UnitTestConstants.APIUserName, header[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual(UnitTestConstants.APIPassword, header[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(UnitTestConstants.APISignature, header[BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER]);
            Assert.AreEqual(UnitTestConstants.ApplicationID, header[BaseConstants.PAYPAL_APPLICATION_ID]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
        }

        [Test]
        public void GetHeaderMapWithCertificateWithTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(AppConfigMgr, AppCredentialMgr, "payload", "servicename", "method", UnitTestConstants.CertificateAPIUserName, "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();            
            string authHeader = header[BaseConstants.PAYPAL_AUTHORIZATION_PLATFORM];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void GetHeaderMapCertificateWithoutTokenTest()
        {
            credential = AppCredentialMgr.GetCredentials(UnitTestConstants.CertificateAPIUserName);
            platformAPIHandler = new PlatformAPICallPreHandler(AppConfigMgr, AppCredentialMgr, "payload", "servicename", "method", credential);
            Dictionary<string, string> header = platformAPIHandler.GetHeaderMap();
            Assert.AreEqual(UnitTestConstants.CertificateAPIUserName, header[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual(UnitTestConstants.CertificateAPIPassword, header[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(UnitTestConstants.ApplicationID, header[BaseConstants.PAYPAL_APPLICATION_ID]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.NVP, header[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
        }  
                
        [Test]
        public void GetPayloadEndpointWithoutTokenTest()
        {
            platformAPIHandler = new PlatformAPICallPreHandler(AppConfigMgr, AppCredentialMgr, "payload", "servicename", "method", UnitTestConstants.APIUserName, "accessToken", "tokenSecret");
            Assert.AreEqual("https://svcs.sandbox.paypal.com/servicename/method", platformAPIHandler.GetEndPoint());
            Assert.AreEqual("payload", platformAPIHandler.GetPayLoad());
            SignatureCredential signatureCredential = (SignatureCredential)platformAPIHandler.GetCredential();
            TokenAuthorization thirdAuth = (TokenAuthorization)signatureCredential.ThirdPartyAuthorization;
            Assert.AreEqual("accessToken", thirdAuth.AccessToken);
            Assert.AreEqual("tokenSecret", thirdAuth.TokenSecret);
        }
    }
}
