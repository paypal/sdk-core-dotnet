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
        /// <returns>A string containing the response from the remote host.</returns>
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
                                case "PUT":
                                case "PATCH":
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
                        // If provided, get and log the response from the remote host.
                        string response = string.Empty;
                        if (ex.Response != null)
                        {
                            using (StreamReader readerStream = new StreamReader(ex.Response.GetResponseStream()))
                            {
                                response = readerStream.ReadToEnd().Trim();
                                logger.Error("Error response:");
                                logger.Error(response);
                            }
                        }
                        logger.Error(ex.Message);

                        // Protocol errors indicate the remote host received the
                        // request, but responded with an error (usually a 4xx or
                        // 5xx error).
                        if (ex.Status == WebExceptionStatus.ProtocolError)
                        {
                            HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;

                            // If the HTTP status code is flagged as one where we
                            // should continue retrying, then ignore the exception
                            // and continue with the retry attempt.
                            if (retryCodes.Contains(statusCode))
                            {
                                continue;
                            }

                            throw new HttpException(ex.Message, response, statusCode, ex.Status);
                        }
                        else if (ex.Status == WebExceptionStatus.Timeout)
                        {
                            // For connection timeout errors, include the connection timeout value that was used.
                            string message = string.Format("{0} (HTTP request timeout was set to {1}ms)", ex.Message, httpRequest.Timeout);
                            throw new ConnectionException(message, response, ex.Status);
                        }

                        // Non-protocol errors indicate something happened with the underlying connection to the server.
                        throw new ConnectionException("Invalid HTTP response " + ex.Message, response, ex.Status);
                    }
                } while (retries++ < retriesConfigured);
            }
            catch (PayPalException)
            {
                // Rethrow any PayPalExceptions since they already contain the
                // details of the exception.
                throw;
            }
            catch (System.Exception ex)
            {
                // Repackage any other exceptions to give a bit more context to
                // the caller.
                throw new PayPalException("Exception in PayPal.HttpConnection.Execute(): " + ex.Message, ex);
            }

            // If we've gotten this far, it means all attempts at sending the
            // request resulted in a failed attempt.
            throw new PayPalException("Retried " + retriesConfigured + " times.... Exception in PayPal.HttpConnection.Execute(). Check log for more details.");
        }
    }
}
