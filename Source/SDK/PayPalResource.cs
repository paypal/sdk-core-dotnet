using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from "net20" for Visual Studio 2005 or "net35" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
using PayPal.Manager;
using PayPal.Exception;
using PayPal.Log;
using System.Text;

namespace PayPal
{
    public abstract class PayPalResource
    {
        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(PayPalResource));

        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });

        /// <summary>
        /// Configures and executes REST call: Supports JSON
        /// </summary>
        /// <typeparam name="T">Generic Type parameter for response object</typeparam>
        /// <param name="apiContext">APIContext object</param>
        /// <param name="httpMethod">HttpMethod type</param>
        /// <param name="resource">URI path of the resource</param>
        /// <param name="payload">JSON request payload</param>
        /// <returns>Response object or null otherwise for void API calls</returns>
        /// <exception cref="PayPal.Exception.HttpException">Thrown if there was an error sending the request.</exception>
        /// <exception cref="PayPal.Exception.PaymentsException">Thrown if an HttpException was raised and contains a Payments API error object.</exception>
        /// <exception cref="PayPal.Exception.PayPalException">Thrown for any other issues encountered. See inner exception for further details.</exception>
        public static T ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, string payload)
        {
            Dictionary<string, string> config = null;
            String authorizationToken = null;
            String resourcePath = null;
            Dictionary<string, string> headersMap = null;
            String requestId = null;
            if (apiContext == null)
            {
                throw new PayPalException("APIContext object is null");
            }

            // Fix config object befor proceeding further
            if (apiContext.Config == null)
            {
                config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
            else
            {
                config = ConfigManager.GetConfigWithDefaults(apiContext.Config);
            }

            // Access Token
            authorizationToken = apiContext.AccessToken;

            // Resource URI path
            resourcePath = resource;

            // Custom HTTP Headers
            headersMap = apiContext.HTTPHeaders;

            // PayPal Request Id
            requestId = apiContext.RequestId;

            // Create an instance of IAPICallPreHandler
            IAPICallPreHandler apiCallPreHandler = CreateIAPICallPreHandler(config, headersMap, authorizationToken, requestId, payload, apiContext.SdkVersion);

            return ConfigureAndExecute<T>(config, apiCallPreHandler, httpMethod, resourcePath);
        }

        /// <summary>
        /// Configures and executes REST call: Supports JSON
        /// </summary>
        /// <typeparam name="T">Generic Type parameter for response object</typeparam>
        /// <param name="accessToken">OAuth AccessToken to be used for the call.</param>
        /// <param name="httpMethod">HttpMethod type</param>
        /// <param name="resource">URI path of the resource</param>
        /// <param name="payload">JSON request payload</param>
        /// <returns>Response object or null otherwise for void API calls</returns>
        [Obsolete("'ConfigureAndExecute<T>(string accessToken, HttpMethod httpMethod, string resource, string payload)' is obsolete: 'The recommended alternative is to pass accessToken to APIContext object and use ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, string payLoad) version.'")]
        public static T ConfigureAndExecute<T>(string accessToken, HttpMethod httpMethod, string resource, string payload)
        {
            APIContext apiContext = new APIContext(accessToken);
            return ConfigureAndExecute<T>(apiContext, httpMethod, resource, null, payload);
        }

        /// <summary>
        /// Configures and executes REST call: Supports JSON
        /// </summary>
        /// <typeparam name="T">Generic Type parameter for response object</typeparam>
        /// <param name="apiContext">APIContext object</param>
        /// <param name="httpMethod">HttpMethod type</param>
        /// <param name="resource">URI path of the resource</param>
        /// <param name="headersMap">HTTP Headers</param>
        /// <param name="payload">JSON request payload</param>
        /// <returns>Response object or null otherwise for void API calls</returns>
        [Obsolete("'ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, Dictionary<string, string> headersMap, string payload)' is obsolete: 'The recommended alternative is to pass Custom HTTP-Headers to APIContext HeadersMap and use ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, string payLoad) version.'")]
        public static T ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, Dictionary<string, string> headersMap, string payload)
        {
            // Code refactored; Calls supported method with only one option to pass
            // Custom HTTP headers through APIContext object
            if (apiContext == null)
            {
                throw new PayPalException("APIContext object is null");
            }

            // Merge headersMap argument with APIContext HeadersMap
            Dictionary<string, string> apiHeaders = apiContext.HTTPHeaders;
            if (apiHeaders == null)
            {
                apiHeaders = new Dictionary<string, string>();
            }
            if (headersMap != null && headersMap.Count > 0)
            {
                foreach (KeyValuePair<string, string> header in headersMap)
                {
                    apiHeaders.Add(header.Key.Trim(), header.Value.Trim());
                }
            }
            apiContext.HTTPHeaders = apiHeaders;
            return ConfigureAndExecute<T>(apiContext, httpMethod, resource, payload);
        }

        /// <summary>
        /// Configures and executes REST call: Supports JSON 
        /// </summary>
        /// <typeparam name="T">Generic Type parameter for response object</typeparam>
        /// <param name="config">Configuration Dictionary</param>
        /// <param name="apiCallPreHandler">IAPICallPreHandler instance</param>
        /// <param name="httpMethod">HttpMethod type</param>
        /// <param name="resourcePath">URI path of the resource</param>
        /// <returns>Response object or null otherwise for void API calls</returns>
        /// <exception cref="PayPal.Exception.HttpException">Thrown if there was an error sending the request.</exception>
        /// <exception cref="PayPal.Exception.PaymentsException">Thrown if an HttpException was raised and contains a Payments API error object.</exception>
        /// <exception cref="PayPal.Exception.PayPalException">Thrown for any other issues encountered. See inner exception for further details.</exception>
        private static T ConfigureAndExecute<T>(Dictionary<string, string> config, IAPICallPreHandler apiCallPreHandler, HttpMethod httpMethod, string resourcePath)
        {
            try
            {
                string response = null;
                Uri uniformResourceIdentifier = null;
                Uri baseUri = null;
                Dictionary<string, string> headersMap = apiCallPreHandler.GetHeaderMap();

                baseUri = new Uri(apiCallPreHandler.GetEndpoint());
                if (Uri.TryCreate(baseUri, resourcePath, out uniformResourceIdentifier))
                {
                    ConnectionManager connMngr = ConnectionManager.Instance;
                    connMngr.GetConnection(config, uniformResourceIdentifier.ToString());
                    HttpWebRequest httpRequest = connMngr.GetConnection(config, uniformResourceIdentifier.ToString());
                    httpRequest.Method = httpMethod.ToString();

                    // Set custom content type (default to [application/json])
                    if (headersMap != null && headersMap.ContainsKey(BaseConstants.ContentTypeHeader))
                    {
                        httpRequest.ContentType = headersMap[BaseConstants.ContentTypeHeader].Trim();
                        headersMap.Remove(BaseConstants.ContentTypeHeader);
                    }
                    else
                    {
                        httpRequest.ContentType = BaseConstants.ContentTypeHeaderJson;
                    }

                    // Set User-Agent HTTP header
                    if (headersMap.ContainsKey(BaseConstants.UserAgentHeader))
                    {
                        // aganzha
                        //iso-8859-1
                        Encoding iso8851 = Encoding.GetEncoding("iso-8859-1", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
                        byte[] bytes = Encoding.Convert(Encoding.UTF8, iso8851, Encoding.UTF8.GetBytes(headersMap[BaseConstants.UserAgentHeader]));
                        httpRequest.UserAgent = iso8851.GetString(bytes);
                        headersMap.Remove(BaseConstants.UserAgentHeader);
                    }

                    // Set Custom HTTP headers
                    foreach (KeyValuePair<string, string> entry in headersMap)
                    {
                        httpRequest.Headers.Add(entry.Key, entry.Value);
                    }

                    foreach (string headerName in httpRequest.Headers)
                    {
                        logger.DebugFormat(headerName + ":" + httpRequest.Headers[headerName]);
                    }

                    // Execute call
                    HttpConnection connectionHttp = new HttpConnection(config);
                    response = connectionHttp.Execute(apiCallPreHandler.GetPayload(), httpRequest);
                    if (typeof(T).Name.Equals("Object"))
                    {
                        return default(T);
                    }
                    else if (typeof(T).Name.Equals("String"))
                    {
                        return (T)Convert.ChangeType(response, typeof(T));
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(response);
                    }
                }
                else
                {
                    throw new PayPalException("Cannot create URL; baseURI=" + baseUri.ToString() + ", resourcePath=" + resourcePath);
                }
            }
            catch (HttpException ex)
            {
                //  Check to see if we have a Payments API error.
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    PaymentsException paymentsEx;
                    if (ex.TryConvertTo<PaymentsException>(out paymentsEx))
                    {
                        throw paymentsEx;
                    }
                }
                throw;
            }
            catch (PayPalException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
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

        private static IAPICallPreHandler CreateIAPICallPreHandler(Dictionary<string, string> config, Dictionary<string, string> headersMap, string authorizationToken, string requestId, string payLoad, SDKVersion sdkVersion)
        {
            RESTAPICallPreHandler restAPICallPreHandler = new RESTAPICallPreHandler(config, headersMap);
            restAPICallPreHandler.AuthorizationToken = authorizationToken;
            restAPICallPreHandler.RequestId = requestId;
            restAPICallPreHandler.Payload = payLoad;
            restAPICallPreHandler.SdkVersion = sdkVersion;
            return (IAPICallPreHandler)restAPICallPreHandler;
        }
    }
}
