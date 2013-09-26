using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.IO;
using PayPal.Exception;
using PayPal.Manager;
using System.Globalization;
using PayPal.Log;

namespace PayPal
{
    public class HttpConnection
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(HttpConnection));

        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.BadGateway
                                                });

        /// <summary>
        /// Dynamic Configuration
        /// </summary>
        private Dictionary<string, string> config;

        public HttpConnection(Dictionary<string, string> config)
        {
            this.config = config;
        }

        /// <summary>
        /// Copying existing HttpWebRequest parameters to newly created HttpWebRequest, can't reuse the same HttpWebRequest for retries.
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="config"></param>
        /// <param name="url"></param>
        /// <returns>HttpWebRequest</returns>
        private HttpWebRequest CopyRequest(HttpWebRequest httpRequest, Dictionary<string, string> config, string url)
        {
            ConnectionManager connMngr = ConnectionManager.Instance;

            HttpWebRequest newHttpRequest = connMngr.GetConnection(config, url);
            newHttpRequest.Method = httpRequest.Method;
            newHttpRequest.Accept = httpRequest.Accept;
            newHttpRequest.ContentType = httpRequest.ContentType;
            if (httpRequest.ContentLength > 0)
            {
                newHttpRequest.ContentLength = httpRequest.ContentLength;
            }
            newHttpRequest.UserAgent = httpRequest.UserAgent;
            newHttpRequest.ClientCertificates = httpRequest.ClientCertificates;
            newHttpRequest = CopyHttpWebRequestHeaders(httpRequest, newHttpRequest);
            return newHttpRequest;
        }

        /// <summary>
        /// Copying existing HttpWebRequest headers into newly created HttpWebRequest
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="newHttpRequest"></param>
        /// <returns>HttpWebRequest</returns>
        private HttpWebRequest CopyHttpWebRequestHeaders(HttpWebRequest httpRequest, HttpWebRequest newHttpRequest)
        {
            string[] allKeys = httpRequest.Headers.AllKeys;
            foreach (string key in allKeys)
            {
                switch (key.ToLower(CultureInfo.InvariantCulture))
                {
                    case "accept":
                    case "connection":
                    case "content-length":
                    case "content-type":
                    case "date":
                    case "expect":
                    case "host":
                    case "if-modified-since":
                    case "range":
                    case "referer":
                    case "transfer-encoding":
                    case "user-agent":
                    case "proxy-connection":
                        break;
                    default:
                        newHttpRequest.Headers[key] = httpRequest.Headers[key];
                        break;
                }
            }
            return newHttpRequest;
        }

        /// <summary>
        /// Executing API calls
        /// </summary>
        /// <param name="payLoad"></param>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public string Execute(string payLoad, HttpWebRequest httpRequest)
        {
            int retriesConfigured = config.ContainsKey(BaseConstants.HttpConnectionRetryConfig) ?
                   Convert.ToInt32(config[BaseConstants.HttpConnectionRetryConfig]) : 0;
            int retries = 0;
            try
            {
                do
                {
                    if (retries > 0)
                    {
                        logger.Info("Retrying....");
                        httpRequest = CopyRequest(httpRequest, config, httpRequest.RequestUri.ToString());
                    }
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
                                logger.Error("Error Response: " + response, ex);
                            }
                            logger.Info("Got " + statusCode.ToString() + " status code from server");
                        }
                        if (!RequiresRetry(ex))
                        {
                            // Server responses in the range of 4xx throw a WebException
                            throw new ConnectionException("Invalid HTTP response " + ex.Message, response);
                        }
                    }
                } while (retries++ < retriesConfigured);
            }
            catch (System.Exception ex)
            {
                throw new PayPalException("Exception in HttpConnection Execute: " + ex.Message, ex);
            }
            throw new PayPalException("Retried " + retriesConfigured + " times.... Exception in HttpConnection Execute. Check log for more details.");
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
