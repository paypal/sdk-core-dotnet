using System;
using System.Collections.Generic;
/* NuGet Install
 * Visual Studio 2008
    * Install log4net -OutputDirectory .\packages
    * Add reference from the folder "net35-full"
 * Visual Studio 2010 or higher
    * Install-Package log4net
    * Reference is auto-added 
*/
using log4net;
using PayPal.Exception;
using PayPal.Authentication;

namespace PayPal.NVP
{
    public class SignatureHttpHeaderAuthStrategy : AbstractSignatureHttpHeaderAuthStrategy
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog logger = LogManagerWrapper.GetLogger(typeof(SignatureHttpHeaderAuthStrategy));

        /// <summary>
        /// SignatureHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public SignatureHttpHeaderAuthStrategy(string endpointURL) : base(endpointURL) { }
        	    
        /// <summary>
        /// Processing TokenAuthorization} using SignatureCredential
        /// </summary>
        /// <param name="signCredential"></param>
        /// <param name="toknAuthorization"></param>
        /// <returns></returns>
        protected internal override Dictionary<string, string> ProcessTokenAuthorization(
                SignatureCredential signCredential, TokenAuthorization toknAuthorization)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {
                OAuthGenerator sigGenerator = new OAuthGenerator(signCredential.UserName, signCredential.Password);
                sigGenerator.SetHTTPMethod(OAuthGenerator.HTTPMethod.POST);
                sigGenerator.SetToken(toknAuthorization.AccessToken);
                sigGenerator.SetTokenSecret(toknAuthorization.TokenSecret);
                string tokenTimeStamp = Timestamp;
                sigGenerator.SetTokenTimestamp(tokenTimeStamp);
                logger.Debug("token = " + toknAuthorization.AccessToken + " tokenSecret=" + toknAuthorization.TokenSecret + " uri=" + endpointURL);
                sigGenerator.SetRequestURI(endpointURL);

                //Compute Signature
                string sign = sigGenerator.ComputeSignature();
                logger.Debug("Permissions signature: " + sign);
                string authorization = "token=" + toknAuthorization.AccessToken + ",signature=" + sign + ",timestamp=" + tokenTimeStamp;
                logger.Debug("Authorization string: " + authorization);
                headers.Add(BaseConstants.PayPalPlatformAuthorizationHeader, authorization);
            }
            catch (OAuthException ae)
            {
                throw ae;
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
