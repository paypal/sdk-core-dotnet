using System;
using System.Collections.Generic;
using PayPal.Exception;
using PayPal.Authentication;
using PayPal.Log;

namespace PayPal.NVP
{
    public class SignatureHttpHeaderAuthStrategy : AbstractSignatureHttpHeaderAuthStrategy
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(SignatureHttpHeaderAuthStrategy));


        /// <summary>
        /// SignatureHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public SignatureHttpHeaderAuthStrategy(string endpointUrl) : base(endpointUrl) { }
        	    
        /// <summary>
        /// Processing TokenAuthorization} using SignatureCredential
        /// </summary>
        /// <param name="signCredential"></param>
        /// <param name="tokenAuthorize"></param>
        /// <returns></returns>
        protected internal override Dictionary<string, string> ProcessTokenAuthorization(
                SignatureCredential signCredential, TokenAuthorization tokenAuthorize)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {
                OAuthGenerator generatorOAuth = new OAuthGenerator(signCredential.UserName, signCredential.Password);
                //generatorOAuth.SetHttpPMethod(HttpMethod.POST);
                generatorOAuth.SetToken(tokenAuthorize.AccessToken);
                generatorOAuth.SetTokenSecret(tokenAuthorize.AccessTokenSecret);
                string tokenTimeStamp = Timestamp;
                generatorOAuth.SetTokenTimestamp(tokenTimeStamp);
                logger.DebugFormat("token = " + tokenAuthorize.AccessToken + " tokenSecret=" + tokenAuthorize.AccessTokenSecret + " uri=" + endpointUrl);
                generatorOAuth.SetRequestUri(endpointUrl);

                //Compute Signature
                string sign = generatorOAuth.ComputeSignature();
                logger.DebugFormat("Permissions signature: " + sign);
                string authorization = "token=" + tokenAuthorize.AccessToken + ",signature=" + sign + ",timestamp=" + tokenTimeStamp;
                logger.DebugFormat("Authorization string: " + authorization);
                headers.Add(BaseConstants.PayPalAuthorizationPlatformHeader, authorization);
            }
            catch (OAuthException oex)
            {
                throw oex;
            }
            return headers;
        }

        /// <summary>
        /// Gets the UTC Timestamp
        /// </summary>
        private static string Timestamp
        {
            get
            {
                TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(span.TotalSeconds).ToString();
            }
        }
    }
}
