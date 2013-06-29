using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install log4net -OutputDirectory .\packages
    * Add reference from "net20-full" for Visual Studio 2005 or "net35-full" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package log4net
    * Reference is auto-added 
*/
using log4net;
using PayPal.Manager;
using PayPal.Exception;
using PayPal.Authentication;

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
        private static ILog logger = LogManagerWrapper.GetLogger(typeof(APIService));

        /// <summary>
        /// Retry codes
        /// </summary>
        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });
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
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }  
            if (logger.IsDebugEnabled)
            {
                foreach (string headerName in httpRequest.Headers)
                {
                    logger.Debug(headerName + ":" + httpRequest.Headers[headerName]);
                }
            }
            //Adding payload to HttpWebRequest object
            using (StreamWriter myWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                myWriter.Write(payload);
                logger.Debug(payload);
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

            //Fire request. Retry if configured to do so
            int numRetries = (this.config.ContainsKey(BaseConstants.HttpConnectionRetryConfig)) ?
                    Convert.ToInt32(config[BaseConstants.HttpConnectionRetryConfig]) : 0;
            int retries = 0;

            do
            {
                try
                {
                    //Calling the plaftform API web service and getting the response
                    using (WebResponse response = httpRequest.GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = sr.ReadToEnd();
                            logger.Debug("Service response");
                            logger.Debug(responseString);
                            return responseString;
                        }
                    }
                }
                //Server responses in the range of 4xx and 5xx throw a WebException
                catch (WebException we)
                {
                    HttpStatusCode statusCode = ((HttpWebResponse)we.Response).StatusCode;

                    logger.Info("Got " + statusCode.ToString() + " response from server");
                    if (!RequiresRetry(we))
                    {
                        throw new ConnectionException("Invalid HTTP response " + we.Message);
                    }
                }
                catch(System.Exception ex)
                {
                    throw ex;
                }
            } while (retries++ < numRetries);
            throw new ConnectionException("Invalid HTTP response");
        }

        /// <summary>
        /// Returns true if a HTTP retry is required
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static bool RequiresRetry(WebException ex)
        {
            if (ex.Status != WebExceptionStatus.ProtocolError)
            {
                return false;
            }
            HttpStatusCode status = ((HttpWebResponse)ex.Response).StatusCode;
            return retryCodes.Contains(status);
        }
    }
}
