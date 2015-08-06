using System.Collections.Specialized;
using System.Web;
#pragma warning disable 618
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal.Testing
{
    [TestClass]
    public class IPNMessageTest
    {
        string ipnMsg = "fees_payer=EACHRECEIVER&payment_request_date=Thu+Dec+06+22%3A50%3A00+PST+2012&transaction[0].is_primary_receiver=false&transaction[0].pending_reason=NONE&cancel_url=http%3A%2F%2Flocalhost%3A9080%2Fadaptivepayments-sample%2Findex.html&status=COMPLETED&transaction_type=Adaptive+Payment+PAY&transaction[0].status=Completed&verify_sign=AM1sBeDL1IjnsgstrDz8f0QWZStzApiXR3gXXjJUE15uzMlXzQmgS-.C&charset=windows-1252&sender_email=jb-us-seller%40paypal.com&log_default_shipping_address_in_transaction=false&transaction[0].amount=USD+2.00&pay_key=AP-70354820B64901803&reverse_all_parallel_payments_on_error=false&ipn_notification_url=https%3A%2F%2Fnpi.pagekite.me%2Fadaptivepaymentssample%2FIPNListener&transaction[0].id=0UJ53158NW5107715&return_url=http%3A%2F%2Flocalhost%3A9080%2Fadaptivepayments-sample%2Findex.html&transaction[0].receiver=platfo_1255612361_per%40gmail.com&transaction[0].id_for_sender_txn=8UL93971B69293341&action_type=PAY&notify_version=UNVERSIONED&transaction[0].status_for_sender_txn=Completed&test_ipn=1";
        NameValueCollection ipnMap = new NameValueCollection();
        
        [TestMethod]
        public void IPNRequest()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(ipnMsg);
            IPNMessage ipn = new IPNMessage(nvc);
            Assert.IsTrue(ipn.Validate());
        }

        [TestMethod]
        public void IPNRequestInvalid()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString("test=tests");
            IPNMessage ipn = new IPNMessage(nvc);
            Assert.IsFalse(ipn.Validate());
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void IPNConfigMissingModeAndIpnEndpoint()
        {
            Encoding ipnEncoding = Encoding.GetEncoding("windows-1252");
            byte[] message = ipnEncoding.GetBytes(ipnMsg);
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            config.Remove(BaseConstants.ApplicationModeConfig);
            config.Remove(BaseConstants.IPNEndpointConfig);
            IPNMessage ipn = new IPNMessage(config, message);
            ipn.Validate();
        }

        [TestMethod]
        public void IPNConfigMissingMode()
        {
            Encoding ipnEncoding = Encoding.GetEncoding("windows-1252");
            byte[] message = ipnEncoding.GetBytes(ipnMsg);
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            config.Remove(BaseConstants.ApplicationModeConfig);
            IPNMessage ipn = new IPNMessage(config, message);
            Assert.IsTrue(ipn.Validate());
        }

        [TestMethod]
        public void IPNConfigMissingIpnEndpoint()
        {
            Encoding ipnEncoding = Encoding.GetEncoding("windows-1252");
            byte[] message = ipnEncoding.GetBytes(ipnMsg);
            Dictionary<string, string> config = ConfigManager.Instance.GetProperties();
            config.Remove(BaseConstants.IPNEndpointConfig);
            IPNMessage ipn = new IPNMessage(config, message);
            Assert.IsTrue(ipn.Validate());
        }

        [TestMethod]
        public void IPNMap()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(ipnMsg);
            IPNMessage ipn = new IPNMessage(nvc);
            NameValueCollection ipnMap = ipn.IpnMap;
            Assert.IsNotNull(ipnMap);
        }

        [TestMethod]
        public void IPNTransaction()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(ipnMsg);
            IPNMessage ipn = new IPNMessage(nvc);
            string transactionType = ipn.TransactionType;
            Assert.AreEqual("Adaptive Payment PAY", transactionType);
        }

        [TestMethod]
        public void IPNParameter()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(ipnMsg);
            IPNMessage ipn = new IPNMessage(nvc);
            string parameter = ipn.IpnValue("fees_payer");
            Assert.AreEqual("EACHRECEIVER", parameter);
        }

        [TestMethod]
        public void IPNMissingParameter()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(ipnMsg);
            IPNMessage ipn = new IPNMessage(nvc);
            string parameter = ipn.IpnValue("not_found");
            Assert.IsTrue(string.IsNullOrEmpty(parameter));
        }

        [TestMethod]
        public void IPNParseQueryString()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(ipnMsg);
            if (nvc.HasKeys())
            {
                ipnMap.Add(nvc.GetKey(0), nvc.Get(0));
            }
            string parameter = ipnMap["fees_payer"];
            Assert.AreEqual("EACHRECEIVER", parameter);
        }
    }
}