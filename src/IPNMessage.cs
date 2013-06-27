using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.IO;
/* NuGet Install
 * Visual Studio 2008
    * Install log4net -OutputDirectory .\packages
    * Add reference from the folder "net35-full"
 * Visual Studio 2010 or higher
    * Install-Package log4net
    * Reference is auto-added 
*/
using log4net;
using PayPal.Exception;
using PayPal.Manager;

namespace PayPal
{
    public class IPNMessage
    {
        /// <summary>
        /// Result from IPN validation call
        /// </summary>
        private bool? IPNValidationResult;
   
        /// <summary>
        /// Name value collection containing incoming IPN message key / value pair
        /// </summary>
        private NameValueCollection NVCMap = new NameValueCollection();

        /// <summary>
        /// Incoming IPN message converted to query string format. Used when validating the IPN message.
        /// </summary>
        private string IPNRequest = string.Empty;

        /// <summary>
        /// Encoding format for IPN messages
        /// </summary>
        private const string IPNEncoding = "windows-1252";

        /// <summary>
        /// SDK configuration parameters
        /// </summary>
        private Dictionary<string, string> Config;

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Logger = LogManagerWrapper.GetLogger(typeof(IPNMessage));

        
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
                        items.Add(string.Concat(key, "=", System.Web.HttpUtility.UrlEncode(nvc[key], Encoding.GetEncoding(IPNEncoding))));
                        NVCMap.Add(key, nvc[key]);
                    }
                    IPNRequest = string.Join("&", items.ToArray())+"&cmd=_notify-validate";
                }
            }
            catch (System.Exception ex)
            {
                Logger.Debug(this.GetType().Name + " : " + ex.Message);
            }
        }

        /// <summary>
        /// IPNMessage constructor
        /// </summary>
        /// <param name="nvc"></param>
        [Obsolete("use IPNMessage(byte[] parameters) instead")]
        public IPNMessage(NameValueCollection nvc)
        {
            this.Config = ConfigManager.Instance.GetProperties();
            this.Initialize(nvc);
        }

        /// <summary>
        /// Construct a new IPNMessage object using dynamic SDK configuration
        /// </summary>
        /// <param name="config">Dynamic SDK configuration parameters</param>
        /// <param name="parameters">byte array read from request</param>
        public IPNMessage(Dictionary<string, string> config, byte[] parameters)
        {
            this.Config = config;
            this.Initialize(HttpUtility.ParseQueryString(Encoding.GetEncoding(IPNEncoding).GetString(parameters), Encoding.GetEncoding(IPNEncoding)));
        }

        /// <summary>
        /// Construct a new IPNMessage object using .Config file based configuration
        /// </summary>
        /// <param name="parameters">byte array read from request</param>
        public IPNMessage(byte[] parameters)
        {
            this.Config = ConfigManager.Instance.GetProperties();
            this.Initialize(HttpUtility.ParseQueryString(Encoding.GetEncoding(IPNEncoding).GetString(parameters), Encoding.GetEncoding(IPNEncoding)));
        }

        /// <summary>
        /// Returns the IPN request validation
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            /// If ipn has been previously validated, do not repeat the validation process.
            if (this.IPNValidationResult != null)
            {
                return this.IPNValidationResult.Value;
            }
            else
            {
                try
                {   
                    string ipnEndpoint = GetIPNEndpoint();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ipnEndpoint);

                    //Set values for the request back
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = IPNRequest.Length;

                    //Send the request to PayPal and get the response
                    StreamWriter streamOut = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding(IPNEncoding));
                    streamOut.Write(IPNRequest);
                    streamOut.Close();
                    StreamReader streamIn = new StreamReader(request.GetResponse().GetResponseStream());
                    string strResponse = streamIn.ReadToEnd();
                    streamIn.Close();

                    if (strResponse.Equals("VERIFIED"))
                    {
                        this.IPNValidationResult = true;
                    }
                    else
                    {
                        Logger.Info("IPN validation failed. Got response: " + strResponse);
                        this.IPNValidationResult = false;
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Info(this.GetType().Name + " : " + ex.Message);

                }
                return this.IPNValidationResult.HasValue ? this.IPNValidationResult.Value : false;
            }
        }

        private string GetIPNEndpoint()
        {
            if(Config.ContainsKey(BaseConstants.IPNEndpointConfig) && !string.IsNullOrEmpty(Config[BaseConstants.IPNEndpointConfig]))
            {
                return Config[BaseConstants.IPNEndpointConfig];
            }
            else if (Config.ContainsKey(BaseConstants.ApplicationModeConfig))
            {
                switch (Config[BaseConstants.ApplicationModeConfig].ToLower())
                {
                    case BaseConstants.SandboxMode:
                        return BaseConstants.IPNSandboxEndpoint;
                    case BaseConstants.LiveMode:
                        return BaseConstants.IPNLiveEndpoint;
                    default:
                        throw new ConfigException("You must configure either the application mode (sandbox/live) or an IPN endpoint");
                }
            }
            else
            {
                throw new ConfigException("You must configure either the application mode (sandbox/live) or an IPN endpoint");
            }
        }

        /// <summary>
        /// Gets the IPN request NameValueCollection
        /// </summary>
        public NameValueCollection IPNMap
        {
            get
            {
                return NVCMap;
            }
        }
      
        /// <summary>
        /// Gets the IPN request parameter value for the given name
        /// </summary>
        /// <param name="ipnName"></param>
        /// <returns></returns>
        public string IPNValue(string ipnName)
        {
            return this.NVCMap[ipnName];
        }
        
        /// <summary>
        /// Gets the IPN request transaction type
        /// </summary>
        public string TransactionType
        {
            get
            {
                return this.NVCMap["txn_type"] != null ? this.NVCMap["txn_type"] :
                    (this.NVCMap["transaction_type"] != null ? this.NVCMap["transaction_type"] : null);
            }
        }
    }
}
