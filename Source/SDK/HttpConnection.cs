using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.IO;
using PayPal.Exception;
using PayPal.Manager;
using PayPal.Log;

namespace PayPal
{
    public class HttpConnection
    {
        /// <summary>
        /// Logger
        /// </summary>
        //private static ILog logger = LogManagerWrapper.GetLogger(typeof(ConnectionManager));
        private static Log4netWrapper logger = Log4netWrapper.GetLogger(typeof(HttpConnection));

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

        public string Execute(string payload, HttpWebRequest httpRequest)
        {
            try
            {
                if (!string.IsNullOrEmpty(payload))
                {
                    switch (httpRequest.Method)
                    {
                        case "POST":
                            using (StreamWriter writerStream = new StreamWriter(httpRequest.GetRequestStream()))
                            {
                                if (!string.IsNullOrEmpty(payload))
                                {
                                    writerStream.Write(payload);
                                    writerStream.Flush();
                                    writerStream.Close();
                                    logger.DebugFormat(payload);
                                }

                            }
                            break;
                        default:
                            break;
                    }
                }

                int retriesConfigured = config.ContainsKey(BaseConstants.HttpConnectionRetryConfig) ?
                    Convert.ToInt32(config[BaseConstants.HttpConnectionRetryConfig]) : 0;
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
                                logger.DebugFormat("Service response");
                                logger.DebugFormat(response);
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
                                logger.Error(ex, "Error Response: " + response);
                            }
                            logger.InfoFormat("Got " + statusCode.ToString() + " status code from server");
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
