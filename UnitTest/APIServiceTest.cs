using System.Threading;
using NUnit.Framework;
using PayPal.NVP;
using PayPal.SOAP;

namespace PayPal.UnitTest
{
    [TestFixture]
    class APIServiceTest : TestsBase
    {
        APIService service;
        IAPICallPreHandler handler;
        DefaultSOAPAPICallHandler defaultSOAPHandler;

        [Test]
        public void MakeRequestUsingNVPCertificateCredential()
        {
            handler = new PlatformAPICallPreHandler(AppConfigMgr, AppCredentialMgr, UnitTestConstants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", UnitTestConstants.CertificateAPIUserName, null, null);
            Thread.Sleep(5000);
            APIService service = new APIService(AppConfigMgr, AppConnMgr);
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }     

        [Test]
        public void MakeRequestUsingNVPSignatureCredential()
        {
            handler = new PlatformAPICallPreHandler(AppConfigMgr, AppCredentialMgr, UnitTestConstants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", UnitTestConstants.APIUserName, null, null);
            Thread.Sleep(5000);
            service = new APIService(AppConfigMgr, AppConnMgr);
            string response = service.MakeRequestUsing(handler);           
            Assert.IsNotNull(response);            
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        //[Test] //To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        [Ignore] 
        public void MakeRequestUsingSOAPSignatureCredential()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(AppConfigMgr, UnitTestConstants.PayloadSOAP, null, null);
            handler = new MerchantAPICallPreHandler(AppConfigMgr, AppCredentialMgr, defaultSOAPHandler, UnitTestConstants.APIUserName, null, null);
            service = new APIService(AppConfigMgr, AppConnMgr);
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }
    }
}
