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
        private string requestUri;
        private string tokenTimestamp;
        private HttpMethod methodHttp;
        private ArrayList queryParameters;

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
            this.methodHttp = HttpMethod.POST;
        }

        /// <summary>
        /// Sets Token to be used to generate signature
        /// </summary>
        /// <param name="token"></param>
        public void SetToken(string token)
        {
            this.token = token;
        }

        /// <summary>
        /// Sets Token secret as received from the Permissions API
        /// </summary>
        /// <param name="secret"></param>
        public void SetTokenSecret(string secret)
        {
            this.tokenSecret = System.Text.Encoding.ASCII.GetBytes(secret);
        }

        /// <summary>
        /// Adds parameter that could be part of URL or POST data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddParameter(string name, string value)
        {
            queryParameters.Add(new Parameter(name, value));
        }

        /// <summary>
        /// Sets URI for signature computation
        /// </summary>
        /// <param name="uri"></param>
        public void SetRequestUri(string uri)
        {
            this.requestUri = NormalizeUri(uri);
        }

        /// <summary>
        /// Sets token Timestamp
        /// </summary>
        /// <param name="tokenTimeStamp"></param>
        public void SetTokenTimestamp(string tokenTimeStamp)
        {
            this.tokenTimestamp = tokenTimeStamp;
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

            if (token == string.Empty || tokenSecret.Length == 0 || requestUri == string.Empty || tokenTimestamp == string.Empty)
            {
                throw new OAuthException(
                        "AuthToken or TokenSecret or Request URI or Timestamp not set.");
            }

            string signature = string.Empty;
            try
            {
                string consumerSec = System.Text.Encoding.GetEncoding(method).GetString(consumerSecret);
                string key = PayPalUrlEncoder.encode(consumerSec, method);
                key += delimiter;
                string tokenSec = System.Text.Encoding.GetEncoding(method).GetString(tokenSecret);
                key += PayPalUrlEncoder.encode(tokenSec, method);
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
                string signatureBase = this.methodHttp + delimiter;
                signatureBase += PayPalUrlEncoder.encode(requestUri, method) + delimiter;
                signatureBase += PayPalUrlEncoder.encode(paramString.ToString(), method);
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
        /// NormalizeUri normalizes the given URI as per OAuth spec
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string NormalizeUri(string uri)
        {
            string normalizedUri = string.Empty, port = string.Empty, scheme = string.Empty, path = string.Empty, authority = string.Empty;
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

                normalizedUri = scheme.ToLower();
                normalizedUri += "://";
                normalizedUri += authority.ToLower();

                if (scheme != null && port.Length > 0)
                {
                    if (scheme.Equals("http") && Convert.ToInt32(port) != 80)
                    {
                        normalizedUri += ":";
                        normalizedUri += port;
                    }
                    else if (scheme.Equals("https") && Convert.ToInt32(port) != 443)
                    {
                        normalizedUri += ":";
                        normalizedUri += port;
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
            normalizedUri += path;
            return normalizedUri;
        }

        /// <summary>
        /// Inner class for representing a name/value pair
        /// Implements custom comparison method for sorting
        /// </summary>
        private class Parameter : IComparable
        {
            private string paramName;
            private string paramValue;

            public Parameter(string paramName, string paramValue)
            {

                this.ParameterName = paramName;
                this.ParameterValue = paramValue;
            }            

            public string ParameterName
            {
                get
                {
                    return this.paramName;
                }
                set
                {
                    this.paramName = value;
                }
            }

            
            public string ParameterValue
            {
                get
                {
                    return this.paramValue;
                }
                set
                {
                    this.paramValue = value;
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
                    returnValue = this.ParameterName.CompareTo(param.ParameterName);
                    // if parameter names are equal then compare parameter values
                    if (returnValue == 0)
                    {
                        returnValue = this.ParameterValue.CompareTo(param.ParameterValue);
                    }
                }
                return returnValue;
            }
        }       
    }
}