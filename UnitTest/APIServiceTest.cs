using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading;
using PayPal.Manager;
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
            handler = new PlatformAPICallPreHandler(ConfigMgr, CredentialMgr, UnitTestConstants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", UnitTestConstants.CertificateAPIUserName, null, null);
            Thread.Sleep(5000);
            APIService service = new APIService(ConfigMgr);
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }     

        [Test]
        public void MakeRequestUsingNVPSignatureCredential()
        {
            handler = new PlatformAPICallPreHandler(ConfigMgr, CredentialMgr, UnitTestConstants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", UnitTestConstants.APIUserName, null, null);
            Thread.Sleep(5000);
            service = new APIService(ConfigMgr);
            string response = service.MakeRequestUsing(handler);           
            Assert.IsNotNull(response);            
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        //[Test] //To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        [Ignore] 
        public void MakeRequestUsingSOAPSignatureCredential()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigMgr, UnitTestConstants.PayloadSOAP, null, null);
            handler = new MerchantAPICallPreHandler(ConfigMgr, CredentialMgr, defaultSOAPHandler, UnitTestConstants.APIUserName, null, null);
            service = new APIService(ConfigMgr);
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }
    }
}
