using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Manager;
using PayPal.NVP;
using PayPal.SOAP;

namespace PayPal
{
    [TestClass]
    public class APIServiceTest
    {
        private APIService Service;
        private IAPICallPreHandler Handler;
        private DefaultSOAPAPICallHandler SOAPHandler;

        [TestMethod]
        public void MakeRequestUsingNVPCertificateCredential()
        {
            Handler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", Constants.CertificateAPIUserName, null, null);
            Thread.Sleep(5000);
            APIService service = new APIService(ConfigManager.Instance.GetProperties());
            string response = service.MakeRequestUsing(Handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }     

        [TestMethod]
        public void MakeRequestUsingNVPSignatureCredential()
        {
            Handler = new PlatformAPICallPreHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadNVP, "AdaptivePayments", "ConvertCurrency", Constants.APIUserName, null, null);
            Thread.Sleep(5000);
            Service = new APIService(ConfigManager.Instance.GetProperties());
            string response = Service.MakeRequestUsing(Handler);           
            Assert.IsNotNull(response);            
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        // [TestMethod] // <!--SOAP--> To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        [Ignore] 
        public void MakeRequestUsingSOAPSignatureCredential()
        {
            SOAPHandler = new DefaultSOAPAPICallHandler(ConfigManager.Instance.GetProperties(), Constants.PayloadSOAP, null, null);
            Handler = new MerchantAPICallPreHandler(ConfigManager.Instance.GetProperties(), SOAPHandler, Constants.APIUserName, null, null);
            Service = new APIService(ConfigManager.Instance.GetProperties());
            string response = Service.MakeRequestUsing(Handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }
    }
}
