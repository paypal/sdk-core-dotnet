using System;
using System.Security.Cryptography;
using System.Collections;
using System.Text;
using PayPal.Exception;
using PayPal.OAuth;

namespace PayPal.Authentication
{
    public class OAuthGenerator
    {
        private static string delimiter = "&";
        private static string separator = "=";
        private static string method = "ASCII";
        private static string version = "1.0";
        private static string authentication = "HMAC-SHA1";
        private string consumerKey;
        private string token;
        private byte[] consumerSecret;
        private byte[] tokenSecret;
        private string requestURI;
        private string tokenTimestamp;
        private HTTPMethod methodHTTP;
        private ArrayList queryParameters;

        public enum HTTPMethod
        {
            GET, HEAD, POST, PUT, UPDATE
        };

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>        
        public OAuthGenerator(string consumerKey, string consumerSecret)
        {
            this.queryParameters = new ArrayList();
            this.consumerKey = consumerKey;
            this.consumerSecret = System.Text.Encoding.ASCII.GetBytes(consumerSecret);
            this.methodHTTP = HTTPMethod.POST;
        }

        /// <summary>
        /// Sets Token to be used to generate signature
        /// </summary>
        /// <param name="token"></param>
        public void setToken(string token)
        {
            this.token = token;
        }

        /// <summary>
        /// Sets Token secret as received from the Permissions API
        /// </summary>
        /// <param name="secret"></param>
        public void setTokenSecret(string secret)
        {
            this.tokenSecret = System.Text.Encoding.ASCII.GetBytes(secret);
        }

        /// <summary>
        /// Adds parameter that could be part of URL or POST data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void addParameter(string name, string value)
        {
            queryParameters.Add(new Parameter(name, value));
        }

        /// <summary>
        /// Sets URI for signature computation
        /// </summary>
        /// <param name="uri"></param>
        public void setRequestURI(string uri)
        {
            this.requestURI = NormalizeURI(uri);
        }

        /// <summary>
        /// Sets token Timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        public void setTokenTimestamp(string timestamp)
        {
            this.tokenTimestamp = timestamp;
        }

        //TODO: Remove me
        public void setHTTPMethod(HTTPMethod method)
        {
            this.methodHTTP = method;
        }

        /// <summary>
        ///  Sets time stamp for signature computation
        /// </summary>
        /// <param name="method"></param>
        public void setHTTPMethod(string method)
        {
            switch (method)
            {
                case "GET":
                    this.methodHTTP = HTTPMethod.GET;
                    break;
                case "POST":
                    this.methodHTTP = HTTPMethod.POST;
                    break;
                case "PUT":
                    this.methodHTTP = HTTPMethod.PUT;
                    break;
                case "UPDATE":
                    this.methodHTTP = HTTPMethod.UPDATE;
                    break;
                default:
                    this.methodHTTP = HTTPMethod.POST;
                    break;
            }
        }

        /// <summary>
        /// Computes OAuth Signature as per OAuth specification using signature
        /// </summary>
        /// <returns></returns>
        public string ComputeSignature()
        {
            if (consumerSecret == null || consumerSecret.Length == 0)
            {
                throw new OAuthException("Consumer Secret or key not set.");
            }

            if (token == string.Empty || tokenSecret.Length == 0
                || requestURI == string.Empty || tokenTimestamp == string.Empty)
            {
                throw new OAuthException(
                        "AuthToken or TokenSecret or Request URI or Timestamp not set.");
            }

            string signature = string.Empty;
            try
            {
                string consumerSec = System.Text.Encoding.GetEncoding(method).GetString(consumerSecret);
                //TODO: Why encode consumersecret twice?
                string key = PayPalURLEncoder.Encode(consumerSec, method);
                key += delimiter;
                string tokenSec = System.Text.Encoding.GetEncoding(method).GetString(tokenSecret);
                key += PayPalURLEncoder.Encode(tokenSec, method);
                StringBuilder paramString = new StringBuilder();
                ArrayList oAuthParams = queryParameters;
                oAuthParams.Add(new Parameter("oauth_consumer_key", consumerKey));
                oAuthParams.Add(new Parameter("oauth_version", version));
                oAuthParams.Add(new Parameter("oauth_signature_method", authentication));
                oAuthParams.Add(new Parameter("oauth_token", token));
                oAuthParams.Add(new Parameter("oauth_timestamp", tokenTimestamp));
                oAuthParams.Sort();
                int numParams = oAuthParams.Count - 1;
                for (int counter = 0; counter <= numParams; counter++)
                {
                    Parameter current = (Parameter)oAuthParams[counter];
                    paramString.Append(current.ParameterName).Append(separator).Append(current.ParameterValue);
                    if (counter < numParams)
                        paramString.Append(delimiter);
                }
                string signatureBase = this.methodHTTP + delimiter;
                signatureBase += PayPalURLEncoder.Encode(requestURI, method) + delimiter;
                signatureBase += PayPalURLEncoder.Encode(paramString.ToString(), method);
                Encoding encoding = System.Text.Encoding.ASCII;
                byte[] encodedKey = encoding.GetBytes(key);
                using (HMACSHA1 keyDigest = new HMACSHA1(encodedKey))
                {
                    Encoding encoding1 = System.Text.Encoding.ASCII;
                    byte[] SignBase = encoding1.GetBytes(signatureBase);

                    byte[] digest = keyDigest.ComputeHash(SignBase);
                    signature = System.Convert.ToBase64String(digest);
                }
            }
            catch (System.Exception e)
            {
                throw new OAuthException(e.Message, e);
            }

            return signature;
        }

        /// <summary>
        /// VerifyOAuthSignature verifies signature against computed signature
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public Boolean VerifyOAuthSignature(string signature)
        {
            string signatureComputed = ComputeSignature();
            return signatureComputed != signature ? false : true;
        }

        /// <summary>
        /// NormalizeURI normalizes the given URI as per OAuth spec
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string NormalizeURI(string uri)
        {
            string normalizedURI = string.Empty, port = string.Empty, scheme = string.Empty, path = string.Empty, authority = string.Empty;
            int i, j, k;

            try
            {
                i = uri.IndexOf(":");
                if (i == -1)
                {
                    throw new OAuthException("Invalid URI.");
                }
                else
                {
                    scheme = uri.Substring(0, i);
                }

                // find next : in URL
                j = uri.IndexOf(":", i + 2);
                if (j != -1)
                {
                    // port has specified in URI
                    authority = uri.Substring(scheme.Length + 3, (j - (scheme.Length + 3)));
                    k = uri.IndexOf("/", j);
                    if (k != -1)
                        port = uri.Substring(j + 1, (k - (j + 1)));
                    else
                        port = uri.Substring(j + 1);
                }
                else
                {
                    // no port specified in uri
                    k = uri.IndexOf("/", scheme.Length + 3);
                    if (k != -1)
                        authority = uri.Substring(scheme.Length + 3, (k - (scheme.Length + 3)));
                    else
                        authority = uri.Substring(scheme.Length + 3);
                }

                if (k != -1)
                    path = uri.Substring(k);

                normalizedURI = scheme.ToLower();
                normalizedURI += "://";
                normalizedURI += authority.ToLower();

                if (scheme != null && port.Length > 0)
                {
                    if (scheme.Equals("http") && Convert.ToInt32(port) != 80)
                    {
                        normalizedURI += ":";
                        normalizedURI += port;
                    }
                    else if (scheme.Equals("https") && Convert.ToInt32(port) != 443)
                    {
                        normalizedURI += ":";
                        normalizedURI += port;
                    }
                }
            }
            catch (FormatException nfe)
            {
                throw new OAuthException("Invalid URI.", nfe);
            }
            catch (ArgumentOutOfRangeException are)
            {
                throw new OAuthException("Out Of Range.", are);
            }
            normalizedURI += path;
            return normalizedURI;
        }

        /// <summary>
        /// Inner class for representing a name/value pair
        /// Implements custom comparison method for sorting
        /// </summary>
        private class Parameter : System.IComparable
        {
            private string paramName;
            private string paramValue;

            public Parameter(string paramName, string paramValue)
            {
                this.paramName = paramName;
                this.paramValue = paramValue;
            }

            public string ParameterName
            {
                get
                {
                    return paramName;
                }
                set
                {
                    paramName = value;
                }
            }

            public string ParameterValue
            {
                get
                {
                    return paramValue;
                }
                set
                {
                    paramValue = value;
                }
            }

            /// <summary>
            /// Compare by name or compare by value if both are equal
            /// </summary>
            /// <param name="objectInstance"></param>
            /// <returns></returns>
            public int CompareTo(Object objectInstance)
            {
                if (!(objectInstance is Parameter))
                {
                    throw new InvalidCastException("This object is not of type Parameter");
                }

                Parameter param = (Parameter)objectInstance;
                int returnValue = 0;
                if (param != null)
                {
                    returnValue = this.paramName.CompareTo(param.ParameterName);
                    // if parameter names are equal then compare parameter values
                    if (returnValue == 0)
                    {
                        returnValue = this.paramValue.CompareTo(param.ParameterValue);
                    }
                }
                return returnValue;
            }
        }

        public static string GenerateTimeStamp()
        {
            TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(span.TotalSeconds).ToString();
        }
    }
}