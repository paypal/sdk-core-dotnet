using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using PayPal.Manager;
using PayPal.Exception;
using PayPal.Authentication;
using PayPal.Log;

namespace PayPal
{
    /// <summary>
    /// Calls the actual Platform API web service for the given Payload and APIProfile settings
    /// </summary>
    public class APIService
    {
        /// <summary>
        /// HTTP Method needs to be set.
        /// </summary>
        private const string RequestMethod = BaseConstants.RequestMethod;

        /// <summary>
        /// X509Certificate
        /// </summary>
        private X509Certificate x509;

        private Dictionary<string, string> config;

        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(APIService));

        /// <summary>
        /// Constructor overload
        /// </summary>
        /// <param name="config"></param>
        public APIService(Dictionary<string, string> config)
        {
            this.config = config;
        }

        /// <summary>
        /// Makes a request to API service
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <returns></returns>
        public string MakeRequestUsing(IAPICallPreHandler apiCallHandler)
        {
            string responseString = string.Empty;
            string uri = apiCallHandler.GetEndpoint();
            Dictionary<string, string> headers = apiCallHandler.GetHeaderMap();
            string payload = apiCallHandler.GetPayload();

            //Constructing HttpWebRequest object                
            ConnectionManager connMngr = ConnectionManager.Instance;
            HttpWebRequest httpRequest = connMngr.GetConnection(this.config, uri);
            httpRequest.Method = RequestMethod;
            if (headers != null && headers.ContainsKey(BaseConstants.ContentTypeHeader))
            {
                httpRequest.ContentType = headers[BaseConstants.ContentTypeHeader].Trim();
                headers.Remove(BaseConstants.ContentTypeHeader);
            }
            if (headers != null && headers.ContainsKey(BaseConstants.UserAgentHeader))
            {
                httpRequest.UserAgent = headers[BaseConstants.UserAgentHeader].Trim();
                headers.Remove(BaseConstants.UserAgentHeader);
            }
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }  

            foreach (string headerName in httpRequest.Headers)
            {
                logger.DebugFormat(headerName + ":" + httpRequest.Headers[headerName]);
            }
           
            if (apiCallHandler.GetCredential() is CertificateCredential)
            {
                CertificateCredential certCredential = (CertificateCredential)apiCallHandler.GetCredential();

                //Load the certificate into an X509Certificate2 object.
                if (((CertificateCredential)certCredential).PrivateKeyPassword.Trim() == string.Empty)
                {
                    x509 = new X509Certificate2(((CertificateCredential)certCredential).CertificateFile);
                }
                else
                {
                    x509 = new X509Certificate2(((CertificateCredential)certCredential).CertificateFile, ((CertificateCredential)certCredential).PrivateKeyPassword);
                }
                httpRequest.ClientCertificates.Add(x509);
            }

            HttpConnection connectionHttp = new HttpConnection(config);
            string response = connectionHttp.Execute(payload, httpRequest);

            return response;
        }
    }
}
