using System.Threading;
using PayPal.Manager;
using PayPal.NVP;
using PayPal.SOAP;

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
    class APIServiceTest
    {
        APIService service;
        IAPICallPreHandler handler;
        DefaultSOAPAPICallHandler defaultSOAPHandler;

        [Test]
        public void MakeRequestUsingNVPCertificateCredential()
        {
            handler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", Constants.CertificateAPIUserName, null, null);
            Thread.Sleep(5000);
            APIService service = new APIService(ConfigManager.Instance.GetProperties());
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        [Test]
        public void MakeRequestUsingNVPSignatureCredential()
        {
            handler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", Constants.APIUserName, null, null);
            Thread.Sleep(5000);
            service = new APIService(ConfigManager.Instance.GetProperties());
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        // [Test] // <!--SOAP--> To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        [Ignore]
        public void MakeRequestUsingSOAPSignatureCredential()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadSOAP, null, null);
            handler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSOAPHandler, Constants.APIUserName, null, null);
            service = new APIService(ConfigManager.Instance.GetProperties());
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
{
    [TestClass]
    public class APIServiceTest
    {
        private APIService service;
        private IAPICallPreHandler handler;
        private DefaultSOAPAPICallHandler defaultSOAPHandler;

        [TestMethod]
        public void MakeRequestUsingNVPCertificateCredential()
        {
            handler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", Constants.CertificateAPIUserName, null, null);
            Thread.Sleep(5000);
            APIService service = new APIService(ConfigManager.Instance.GetProperties());
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        [TestMethod]
        public void MakeRequestUsingNVPSignatureCredential()
        {
            handler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", Constants.APIUserName, null, null);
            Thread.Sleep(5000);
            service = new APIService(ConfigManager.Instance.GetProperties());
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        // [TestMethod] // <!--SOAP--> To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        [Ignore]
        public void MakeRequestUsingSOAPSignatureCredential()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadSOAP, null, null);
            handler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), defaultSOAPHandler, Constants.APIUserName, null, null);
            service = new APIService(ConfigManager.Instance.GetProperties());
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }
    }
}
#endif
