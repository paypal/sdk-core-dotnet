using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.Log;
using PayPal.Exception;

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
                System.Exception x509LoadFromFileException = null;
                X509Certificate2 x509 = null;

                try
                {
                    //Load the certificate into an X509Certificate2 object.
                    if (string.IsNullOrEmpty(certCredential.PrivateKeyPassword.Trim()))
                    {
                        x509 = new X509Certificate2(certCredential.CertificateFile);
                    }
                    else
                    {
                        x509 = new X509Certificate2(certCredential.CertificateFile, certCredential.PrivateKeyPassword);
                    }
                }
                catch (System.Exception ex)
                {
                    x509LoadFromFileException = ex;
                }

                // If we failed to load the certificate from the specified file,
                // then try loading it from the certificates store using the provided UserName.
                if (x509 == null)
                {
                    // Start by checking the local machine store.
                    x509 = this.GetX509CertificateForUserName(certCredential.UserName, StoreLocation.LocalMachine);
                }

                // If the certificate couldn't be found in the LM store, check
                // the current user store.
                if (x509 == null)
                {
                    x509 = this.GetX509CertificateForUserName(certCredential.UserName, StoreLocation.CurrentUser);
                }

                // If the certificate still hasn't been loaded by this point,
                // then it means it failed to be loaded from a file and was not
                // found in any of the certificate stores.
                if (x509 == null)
                {
                    throw new PayPalException("Failed to load the certificate file", x509LoadFromFileException);
                }

                httpRequest.ClientCertificates.Add(x509);
            }

            HttpConnection connectionHttp = new HttpConnection(config);
            string response = connectionHttp.Execute(payload, httpRequest);

            return response;
        }

        /// <summary>
        /// Searches the specified X509 store for a certificate that matches the specified user name.
        /// </summary>
        /// <param name="userName">The user name of the certificate to look for.</param>
        /// <param name="location">The X509 certificate store to look through.</param>
        /// <returns>An X509Certificate2 object if found; null otherwise.</returns>
        private X509Certificate2 GetX509CertificateForUserName(string userName, StoreLocation location)
        {
            X509Store store = new X509Store(location);
            X509Certificate2 x509 = null;

            // Open the X509Store as read-only since we're only trying to search
            // for a certificate in the store.
            store.Open(OpenFlags.ReadOnly);

            foreach (X509Certificate2 storeCert in store.Certificates)
            {
                string certName = storeCert.GetNameInfo(X509NameType.DnsName, false);
                if (certName.Equals(userName))
                {
                    x509 = storeCert;
                    break;
                }
            }

            // Close the store.
            //
            // NOTE: X509Store was updated to inherit IDisposable in .NET 4.5.
            //       If/When this code is ever updated to support .NET 4.5 and
            //       later, consider replacing this with a using-statement to
            //       ensure any unmanaged resources are properly cleaned up.
            store.Close();

            return x509;
        }
    }
}
