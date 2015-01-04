using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;
using PayPal.Log;

namespace PayPal.SOAP
{
    public class SignatureHttpHeaderAuthStrategy : AbstractSignatureHttpHeaderAuthStrategy
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(SignatureHttpHeaderAuthStrategy));
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpointUrl"></param>
        public SignatureHttpHeaderAuthStrategy(string endpointUrl) : base(endpointUrl) { }

	    /// <summary>
        /// Processing for TokenAuthorization using SignatureCredential
	    /// </summary>
	    /// <param name="signCredential"></param>
	    /// <param name="tokenAuthorize"></param>
	    /// <returns></returns>
	    protected internal override Dictionary<string, string> ProcessTokenAuthorization(SignatureCredential signCredential, TokenAuthorization tokenAuthorize)
    	{
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {   
                OAuthGenerator signGenerator = new OAuthGenerator(signCredential.UserName, signCredential.Password);
                signGenerator.SetToken(tokenAuthorize.AccessToken);
                signGenerator.SetTokenSecret(tokenAuthorize.AccessTokenSecret);
                string tokenTimeStamp = Timestamp;
                signGenerator.SetTokenTimestamp(tokenTimeStamp);
                logger.DebugFormat("token = " + tokenAuthorize.AccessToken + " tokenSecret=" + tokenAuthorize.AccessTokenSecret + " uri=" + endpointUrl);
                signGenerator.SetRequestUri(endpointUrl);
                
                //Compute Signature
                string sign = signGenerator.ComputeSignature();
                logger.DebugFormat("Permissions signature: " + sign);
                string authorization = "token=" + tokenAuthorize.AccessToken + ",signature=" + sign + ",timestamp=" + tokenTimeStamp;
                logger.DebugFormat("Authorization string: " + authorization);
                headers.Add(BaseConstants.PayPalAuthorizationMerchantHeader, authorization);
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
