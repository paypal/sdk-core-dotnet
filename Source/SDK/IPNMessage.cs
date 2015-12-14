using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.IO;
using PayPal.Exception;
using PayPal.Manager;
using PayPal.Log;

namespace PayPal
{
    public class IPNMessage
    {
        /// <summary>
        /// Result from IPN validation call
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
        private Encoding ipnEncoding = Encoding.GetEncoding("windows-1252");

        /// <summary>
        /// SDK configuration parameters
        /// </summary>
        private Dictionary<string, string> config;

        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(IPNMessage));
                
        /// <summary>
        /// Initializing nvcMap and constructing query string
        /// </summary>
        /// <param name="nvc"></param>
        private void Initialize(NameValueCollection nvc)
        {
            List<string> items = new List<string>();
            try
            {
                if (nvc.HasKeys())
                {
                    foreach (string key in nvc.Keys)
                    {
                        items.Add(string.Concat(key, "=", System.Web.HttpUtility.UrlEncode(nvc[key], ipnEncoding)));
                        nvcMap.Add(key, nvc[key]);
                    }
                    ipnRequest = string.Join("&", items.ToArray())+"&cmd=_notify-validate";
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(this.GetType().Name + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// IPNMessage constructor
        /// </summary>
        /// <param name="nvc"></param>
        //[Obsolete("'IPNMessage(NameValueCollection nvc)' is obsolete: 'The recommended alternative is IPNMessage(byte[] parameters).'")]
        public IPNMessage(NameValueCollection nvc)
        {
            this.config = ConfigManager.Instance.GetProperties();
            this.Initialize(nvc);
        }

        /// <summary>
        /// Construct a new IPNMessage object using dynamic SDK configuration
        /// </summary>
        /// <param name="config">Dynamic SDK configuration parameters</param>
        /// <param name="parameters">byte array read from request</param>
        public IPNMessage(Dictionary<string, string> config, byte[] parameters)
        {
            this.config = config;
            this.Initialize(HttpUtility.ParseQueryString(ipnEncoding.GetString(parameters), ipnEncoding));
        }

        /// <summary>
        /// Construct a new IPNMessage object using .Config file based configuration
        /// </summary>
        /// <param name="parameters">byte array read from request</param>
        public IPNMessage(byte[] parameters)
        {
            this.config = ConfigManager.Instance.GetProperties();
            this.Initialize(HttpUtility.ParseQueryString(ipnEncoding.GetString(parameters), ipnEncoding));
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
                // Get the IPN endpoint to use in order to validate the received IPN.
                // NOTE: This call will throw an exception if the config is not setup properly.
                string ipnEndpoint = this.GetIPNEndpoint();

                try
                {
                    // Setup the HTTP request to use for posting the IPN back to PayPal.
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ipnEndpoint);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = this.ipnRequest.Length;

                    using (StreamWriter streamOut = new StreamWriter(request.GetRequestStream(), ipnEncoding))
                    {
                        streamOut.Write(this.ipnRequest);
                    }

                    // Send the request to PayPal and get the response
                    string strResponse = string.Empty;
                    using (StreamReader streamIn = new StreamReader(request.GetResponse().GetResponseStream()))
                    {
                        strResponse = streamIn.ReadToEnd();
                    }

                    // If the IPN is valid, the response from PayPal will be 'VERIFIED'.
                    if (strResponse.Equals("VERIFIED"))
                    {
                        this.ipnValidationResult = true;
                    }
                    else
                    {
                        logger.InfoFormat("IPN validation failed. Got response: " + strResponse);
                        this.ipnValidationResult = false;
                    }
                }
                catch (System.Exception ex)
                {
                    logger.InfoFormat(this.GetType().Name + " : " + ex.Message);

                }
                return this.ipnValidationResult.HasValue ? this.ipnValidationResult.Value : false;
            }
        }

        private string GetIPNEndpoint()
        {
            if(config.ContainsKey(BaseConstants.IPNEndpointConfig) && !string.IsNullOrEmpty(config[BaseConstants.IPNEndpointConfig]))
            {
                return config[BaseConstants.IPNEndpointConfig];
            }
            else if (config.ContainsKey(BaseConstants.ApplicationModeConfig))
            {
                switch (config[BaseConstants.ApplicationModeConfig].ToLower())
                {
                    case BaseConstants.SandboxMode:
                        return BaseConstants.IPNSandboxEndpoint;

                    case BaseConstants.TestSandboxMode:
                        return BaseConstants.IPNTestSandboxEndpoint;

                    case BaseConstants.LiveMode:
                        return BaseConstants.IPNLiveEndpoint;
                }
            }

            throw new ConfigException("IPN endpoint could not be determined based on the current PayPal configuration settings. The configuration must either define 'mode' (sandbox/live) or 'IPNEndpoint'.");
        }

        //TODO: To be renamed as 'IPNMap' as per .NET Naming Conventions
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

        //TODO: To be renamed as 'IPNValue' as per .NET Naming Conventions
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
