using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.IO;
using log4net;
using PayPal.Manager;

namespace PayPal
{
    public class IPNMessage
    {
        
        /// <summary>
        /// Result from ipn validation call
        /// </summary>
        private bool? ipnValidationResult;        
        /// <summary>
        /// Name value collection containing incoming IPN message key / value pair
        /// </summary>
        private NameValueCollection nvcMap = new NameValueCollection();
        /// <summary>
        /// Incoming IPN message converted to query string format. Used when validating the IPN message.
        /// </summary>
        private string ipnRequest = string.Empty;

        /// <summary>
        /// Encoding format for IPN messages
        /// </summary>
        private const string IPNEncoding = "windows-1252";

        private readonly ConfigManager configMgr;

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog logger = LogManagerWrapper.GetLogger(typeof(IPNMessage));

        
        /// <summary>
        /// Initializing nvcMap and constructing query string
        /// </summary>
        /// <param name="nvc"></param>
        private void initialize(NameValueCollection nvc){
            List<string> items = new List<string>();
        try
            {
                if (nvc.HasKeys())
                {
                    foreach (string key in nvc.Keys)
                    {
                        items.Add(string.Concat(key, "=", System.Web.HttpUtility.UrlEncode(nvc[key], Encoding.GetEncoding(IPNEncoding))));
                        nvcMap.Add(key, nvc[key]);
                    }
                    ipnRequest = string.Join("&", items.ToArray())+"&cmd=_notify-validate";
                }
            }
            catch (System.Exception ex)
            {
                logger.Debug(this.GetType().Name + " : " + ex.Message);
            }
        }


        /// <summary>
        /// IPNMessage constructor
        /// </summary>
        /// <param name="nvc"></param>
        [Obsolete("use IPNMessage(byte[] parameters) instead")]
        public IPNMessage(ConfigManager configMgr, NameValueCollection nvc)
        {
            this.configMgr = configMgr;
            this.initialize(nvc);
        }
        /// <summary>
        /// IPNMessage constructor
        /// </summary>
        /// <param name="parameters">byte array read from request</param>
        public IPNMessage(ConfigManager configMgr, byte[] parameters)
        {
            this.configMgr = configMgr;
            this.initialize(HttpUtility.ParseQueryString(Encoding.GetEncoding(IPNEncoding).GetString(parameters), Encoding.GetEncoding(IPNEncoding)));
        }

        /// <summary>
        /// Returns the IPN request validation
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            /// If ipn has been previously validated, do not repeat the validation process.
            if (this.ipnValidationResult != null)
            {
                return this.ipnValidationResult.Value;
            }
            else
            {
                try
                {
                    string ipnEndpoint = configMgr.GetProperty(BaseConstants.IPNEndpoint);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ipnEndpoint);

                    //Set values for the request back
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = ipnRequest.Length;

                    //Send the request to PayPal and get the response
                    StreamWriter streamOut = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding(IPNEncoding));
                    streamOut.Write(ipnRequest);
                    streamOut.Close();
                    StreamReader streamIn = new StreamReader(request.GetResponse().GetResponseStream());
                    string strResponse = streamIn.ReadToEnd();
                    streamIn.Close();

                    if (strResponse.Equals("VERIFIED"))
                    {
                        this.ipnValidationResult = true;
                    }
                    else
                    {
                        logger.Info("IPN validation failed. Got response: " + strResponse);
                        this.ipnValidationResult = false;
                    }
                }
                catch (System.Exception ex)
                {
                    logger.Info(this.GetType().Name + " : " + ex.Message);

                }
                return this.ipnValidationResult.HasValue ? this.ipnValidationResult.Value : false;
            }
        }

        /// <summary>
        /// Gets the IPN request NameValueCollection
        /// </summary>
        public NameValueCollection IpnMap
        {
            get
            {
                return nvcMap;
            }
        }
      
        /// <summary>
        /// Gets the IPN request parameter value for the given name
        /// </summary>
        /// <param name="ipnName"></param>
        /// <returns></returns>
        public string IpnValue(string ipnName)
        {
            return this.nvcMap[ipnName];
        }
        
        /// <summary>
        /// Gets the IPN request transaction type
        /// </summary>
        public string TransactionType
        {
            get
            {
                return this.nvcMap["txn_type"] != null ? this.nvcMap["txn_type"] :
                    (this.nvcMap["transaction_type"] != null ? this.nvcMap["transaction_type"] : null);
            }
        }
    }
}
