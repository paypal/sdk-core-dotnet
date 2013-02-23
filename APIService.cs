using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
        private const string RequestMethod = BaseConstants.REQUESTMETHOD;

        /// <summary>
        /// X509Certificate
        /// </summary>
        private X509Certificate x509;

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog logger = LogManagerWrapper.GetLogger(typeof(APIService));

        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });

        private readonly IConfigManager _configMgr;
        private readonly ConnectionManager _connManager;

        public APIService(IConfigManager configMgr, ConnectionManager connMgr)
        {
            _configMgr = configMgr;
            _connManager = connMgr;
        }

        /// <summary>
        /// Makes a request to API service
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <returns></returns>
        public string MakeRequestUsing(IAPICallPreHandler apiCallHandler)
        {
            string responseString = string.Empty;
            string uri = apiCallHandler.GetEndPoint();
            Dictionary<string, string> headers = apiCallHandler.GetHeaderMap();
            string payLoad = apiCallHandler.GetPayLoad();

            // Constructing HttpWebRequest object                
            HttpWebRequest httpRequest = _connManager.GetConnection(uri);
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
            // Adding payLoad to HttpWebRequest object
            using (StreamWriter myWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                myWriter.Write(payLoad);
                logger.Debug(payLoad);
            }

            if (apiCallHandler.GetCredential() is CertificateCredential)
            {
                CertificateCredential certCredential = (CertificateCredential)apiCallHandler.GetCredential();

                // Load the certificate into an X509Certificate2 object.
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

            // Fire request. Retry if configured to do so
            int numRetries = (_configMgr.GetProperty(BaseConstants.HTTP_CONNECTION_RETRY) != null) ?
                Convert.ToInt32(_configMgr.GetProperty(BaseConstants.HTTP_CONNECTION_RETRY)) : 0;
            int retries = 0;

            do
            {
                try
                {
                    // Calling the plaftform API web service and getting the response
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
                // Server responses in the range of 4xx and 5xx throw a WebException
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
