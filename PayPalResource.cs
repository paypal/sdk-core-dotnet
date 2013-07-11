using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
// NuGet Install
// install PayPalCoreSDK -excludeversion -outputDirectory .\Packages
// 2.0
using log4net;
using PayPal.Exception;
// install Newtonsoft.Json -excludeversion -outputDirectory .\Packages
// net35
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayPal.Manager;

namespace PayPal
{
    public abstract class PayPalResource
    {
        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static readonly ILog logger = LogManagerWrapper.GetLogger(typeof(PayPalResource));

        /// <summary>
        /// Codes for which HTTP calls are retried
        /// </summary>
        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });
        /// <summary>
        /// Product ID
        /// </summary>
        public const string SdkID = "rest-sdk-dotnet";

        /// <summary>
        /// Product Version
        /// </summary>
        public const string SdkVersion = "0.7.1";

        /// <summary>
        /// Configures and executes REST call: Supports JSON
        /// </summary>
        /// <typeparam name="T">Generic Type parameter for response object</typeparam>
        /// <param name="apiContext">APIContext object</param>
        /// <param name="httpMethod">HttpMethod type</param>
        /// <param name="resource">URI path of the resource</param>
        /// <param name="payLoad">JSON request payload</param>
        /// <returns>Response object or null otherwise for void API calls</returns>
        public static T ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, string payLoad)
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
                config = ConfigManager.getConfigWithDefaults(ConfigManager.Instance.GetProperties());
            }
            else
            {
                config = ConfigManager.getConfigWithDefaults(apiContext.Config);
            }

            // Access Token
            authorizationToken = apiContext.AccessToken;

            // Resource URI path
            resourcePath = resource;

            // Custom HTTP Headers
            headersMap = apiContext.HeadersMap;

            // PayPal Request Id
            requestId = apiContext.RequestID;

            // Create an instance of IAPICallPreHandler
            IAPICallPreHandler apiCallPreHandler = createIAPICallPreHandler(config, headersMap, authorizationToken, requestId, payLoad);

            return ConfigureAndExecute<T>(config, apiCallPreHandler, httpMethod, resourcePath);
        }

        /// <summary>
        /// Configures and executes REST call: Supports JSON
        /// </summary>
        /// <typeparam name="T">Generic Type parameter for response object</typeparam>
        /// <param name="accessToken">OAuth AccessToken to be used for the call.</param>
        /// <param name="httpMethod">HttpMethod type</param>
        /// <param name="resource">URI path of the resource</param>
        /// <param name="payLoad">JSON request payload</param>
        /// <returns>Response object or null otherwise for void API calls</returns>
        [Obsolete("Pass accessToken to APIContext object and use ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, string payLoad) version")]
        public static T ConfigureAndExecute<T>(string accessToken, HttpMethod httpMethod, string resource, string payLoad)
        {
            APIContext apiContext = new APIContext(accessToken);
            return ConfigureAndExecute<T>(apiContext, httpMethod, resource, null, payLoad);
        }

        /// <summary>
        /// Configures and executes REST call: Supports JSON
        /// </summary>
        /// <typeparam name="T">Generic Type parameter for response object</typeparam>
        /// <param name="apiContext">APIContext object</param>
        /// <param name="httpMethod">HttpMethod type</param>
        /// <param name="resource">URI path of the resource</param>
        /// <param name="headersMap">HTTP Headers</param>
        /// <param name="payLoad">JSON request payload</param>
        /// <returns>Response object or null otherwise for void API calls</returns>
        [Obsolete("Pass Custom HTTP-Headers to APIContext HeadersMap and use ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, string payLoad) version")]
        public static T ConfigureAndExecute<T>(APIContext apiContext, HttpMethod httpMethod, string resource, Dictionary<string, string> headersMap, string payLoad)
        {

            // Code refactored; Calls supported method with only one option to pass
            // Custom HTTP headers through APIContext object
            if (apiContext == null)
            {
                throw new PayPalException("APIContext object is null");
            }

            // Merge headersMap argument with APIContext HeadersMap
            Dictionary<string, string> apiHeaders = apiContext.HeadersMap;
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
            apiContext.HeadersMap = apiHeaders;
            return ConfigureAndExecute<T>(apiContext, httpMethod, resource, payLoad);
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
        private static T ConfigureAndExecute<T>(Dictionary<string, string> config, IAPICallPreHandler apiCallPreHandler, HttpMethod httpMethod, string resourcePath)
        {
            try
            {
                string response = null;
                Uri uniformResourceIdentifier = null;
                Uri baseUri = null;
                Dictionary<string, string> headersMap = apiCallPreHandler.GetHeaderMap();

                baseUri = new Uri(apiCallPreHandler.GetEndPoint());
                if (Uri.TryCreate(baseUri, resourcePath, out uniformResourceIdentifier))
                {
                    ConnectionManager connMngr = ConnectionManager.Instance;
                    connMngr.GetConnection(config, uniformResourceIdentifier.ToString());
                    HttpWebRequest httpRequest = connMngr.GetConnection(config, uniformResourceIdentifier.ToString());
                    httpRequest.Method = httpMethod.ToString();

                    // Set custom content type (default to [application/json])
                    if (headersMap != null && headersMap.ContainsKey(BaseConstants.CONTENT_TYPE_HEADER))
                    {
                        httpRequest.ContentType = headersMap[BaseConstants.CONTENT_TYPE_HEADER].Trim();
                        headersMap.Remove(BaseConstants.CONTENT_TYPE_HEADER);
                    }
                    else
                    {
                        httpRequest.ContentType = BaseConstants.CONTENT_TYPE_JSON;
                    }

                    // Set User-Agent HTTP header
                    if (headersMap.ContainsKey(BaseConstants.USER_AGENT_HEADER))
                    {
                        httpRequest.UserAgent = headersMap[BaseConstants.USER_AGENT_HEADER];
                        headersMap.Remove(BaseConstants.USER_AGENT_HEADER);
                    }

                    // Set Custom HTTP headers
                    foreach (KeyValuePair<string, string> entry in headersMap)
                    {
                        httpRequest.Headers.Add(entry.Key, entry.Value);
                    }

                    // Debugging
                    if (logger.IsDebugEnabled)
                    {
                        foreach (string headerName in httpRequest.Headers)
                        {
                            logger.Debug(headerName + ":" + httpRequest.Headers[headerName]);
                        }
                    }

                    // Execute call
                    HttpConnection connectionHttp = new HttpConnection(config);
                    response = connectionHttp.Execute(apiCallPreHandler.GetPayLoad(), httpRequest);
                    if (typeof(T).Name.Equals("Object"))
                    {
                        return default(T);
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
            catch (PayPalException ex)
            {
                throw ex;
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

        private static IAPICallPreHandler createIAPICallPreHandler(Dictionary<string, string> config, Dictionary<string, string> headersMap, string authorizationToken, string requestId, string payLoad)
        {
            RESTAPICallPreHandler restAPICallPreHandler = new RESTAPICallPreHandler(config, headersMap);
            restAPICallPreHandler.authorizationToken = authorizationToken;
            restAPICallPreHandler.requestId = requestId;
            restAPICallPreHandler.payLoad = payLoad;
            return (IAPICallPreHandler)restAPICallPreHandler;
        }
    }
}
