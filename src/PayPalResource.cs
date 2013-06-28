using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
/* NuGet Install
 * Visual Studio 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from the folder "net35"
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;
/* NuGet Install
 * Visual Studio 2008
    * Install log4net -OutputDirectory .\packages
    * Add reference from the folder "net35-full"
 * Visual Studio 2010 or higher
    * Install-Package log4net
    * Reference is auto-added 
*/
using log4net;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal
{
    public abstract class PayPalResource
    {
        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static ILog logger = LogManagerWrapper.GetLogger(typeof(PayPalResource));

        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });

        public const string SDKID = "rest-sdk-dotnet";

        public const string SDKVersion = "0.7.1";

        public static T ConfigureAndExecute<T>(string accessToken, HttpMethod httpMethod, string resource, string payLoad)
        {
            apiContext apiContext = new apiContext(accessToken);
            return ConfigureAndExecute<T>(apiContext, httpMethod, resource, null, payLoad);
        }

        public static T ConfigureAndExecute<T>(apiContext apiContext, HttpMethod httpMethod, string resource, string payLoad)
        {
            return ConfigureAndExecute<T>(apiContext, httpMethod, resource, null, payLoad);
        }

        public static T ConfigureAndExecute<T>(apiContext apiContext, HttpMethod httpMethod, string resource, Dictionary<string, string> headersMap, string payLoad)
        {
            try
            {
                string response = null;
                Dictionary<string, string> headers;
                Uri uniformResourceIdentifier = null;
                Uri baseUri = null;
                Dictionary<string, string> config = null;
                if (apiContext.Config == null)
                {
                    config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
                }
                else
                {
                    config = ConfigManager.GetConfigWithDefaults(apiContext.Config);
                }
                baseUri = GetBaseURI(config);
                bool success = Uri.TryCreate(baseUri, resource, out uniformResourceIdentifier);

                RESTConfiguration restConfiguration = new RESTConfiguration(config, headersMap);
                restConfiguration.AuthorizationToken = apiContext.AccessToken;
                restConfiguration.RequestID = apiContext.RequestID;
                headers = restConfiguration.GetHeaders();

                ConnectionManager connMngr = ConnectionManager.Instance;
                connMngr.GetConnection(config, uniformResourceIdentifier.ToString());
                HttpWebRequest httpRequest = connMngr.GetConnection(config, uniformResourceIdentifier.ToString());
                httpRequest.Method = httpMethod.ToString();
                if (headersMap != null && headersMap.ContainsKey("Content-Type") && headersMap["Content-Type"].Equals("application/x-www-form-urlencoded"))
                {
                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    httpRequest.ContentType = "application/json";
                }
                httpRequest.ContentLength = payLoad.Length;
                foreach (KeyValuePair<string, string> header in headers)
                {
                    if (header.Key.Trim().Equals("User-Agent"))
                    {
                        httpRequest.UserAgent = header.Value;
                    }
                    else
                    {
                        httpRequest.Headers.Add(header.Key, header.Value);
                    }
                }
                if (logger.IsDebugEnabled)
                {
                    foreach (string headerName in httpRequest.Headers)
                    {
                        logger.Debug(headerName + ":" + httpRequest.Headers[headerName]);
                    }
                }
                HttpConnection connectionHttp = new HttpConnection(config);
                response = connectionHttp.Execute(payLoad, httpRequest);
                if (typeof(T).Name.Equals("Object"))
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(response);
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

        /// <summary>
        /// Returns Base URI required for the call
        /// </summary>
        /// <param name="config">configuration map</param>
        /// <returns>Uri</returns>
        private static Uri GetBaseURI(Dictionary<string, string> config)
        {
            Uri baseURI = null;
            if (config.ContainsKey("endpoint"))
            {
                baseURI = new Uri(config["endpoint"]);
            }
            else if (config.ContainsKey("mode"))
            {
                switch (config["mode"].ToLower())
                {
                    case "sandbox":
                        baseURI = new Uri(BaseConstants.RESTSandboxEndpoint);
                        break;
                    case "live":
                        baseURI = new Uri(BaseConstants.RESTLiveEndpoint);
                        break;
                }
            }
            return baseURI;
        }
    }
}
