using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
// NuGet Install
// install PayPalCoreSDK -excludeversion -outputDirectory .\Packages
// 2.0
using log4net;
using PayPal.Exception;
using PayPal.Manager;

namespace PayPal
{
    public class HttpConnection
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static ILog logger = LogManagerWrapper.GetLogger(typeof(ConnectionManager));

        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });

        /// <summary>
        /// Dynamic Configuration
        /// </summary>
        private Dictionary<string, string> config;

        public HttpConnection(Dictionary<string, string> config)
        {
            this.config = config;
        }

        public string Execute(string payLoad, HttpWebRequest httpRequest)
        {
            try
            {
                if (!string.IsNullOrEmpty(payLoad))
                {
                    switch (httpRequest.Method)
                    {
                        case "POST":
                            using (StreamWriter writerStream = new StreamWriter(httpRequest.GetRequestStream()))
                            {
                                if (!string.IsNullOrEmpty(payLoad))
                                {
                                    writerStream.Write(payLoad);
                                    writerStream.Flush();
                                    writerStream.Close();
                                    logger.Debug(payLoad);
                                }

                            }
                            break;
                        default:
                            break;
                    }
                }

                int retriesConfigured = config.ContainsKey(BaseConstants.HTTP_CONNECTION_RETRY_CONFIG) ?
                    Convert.ToInt32(config[BaseConstants.HTTP_CONNECTION_RETRY_CONFIG]) : 0;
                int retries = 0;

                do
                {
                    try
                    {
                        using (WebResponse responseWeb = httpRequest.GetResponse())
                        {
                            using (StreamReader readerStream = new StreamReader(responseWeb.GetResponseStream()))
                            {
                                string response = readerStream.ReadToEnd().Trim();
                                logger.Debug("Service response");
                                logger.Debug(response);
                                return response;
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        string response = null;
                        if (ex.Response is HttpWebResponse)
                        {
                            HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                            using (StreamReader readerStream = new StreamReader(ex.Response.GetResponseStream()))
                            {
                                response = readerStream.ReadToEnd().Trim();
                                logger.Error("Error Response: " + response);
                            }
                            logger.Info("Got " + statusCode.ToString() + " status code from server");
                        }
                        if (!RequiresRetry(ex))
                        {
                            // Server responses in the range of 4xx and 5xx throw a WebException
                            throw new ConnectionException("Invalid HTTP response " + ex.Message, response);
                        }
                    }

                } while (retries++ < retriesConfigured);
            }
            catch (System.Exception ex)
            {
                throw new PayPalException("Exception in HttpConnection Execute: " + ex.Message, ex);
            }
            throw new PayPalException("Exception in HttpConnection Execute");
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
