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
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal.NVP
{
    public class CertificateHttpHeaderAuthStrategy : AbstractCertificateHttpHeaderAuthStrategy 
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog logger = LogManagerWrapper.GetLogger(typeof(CertificateHttpHeaderAuthStrategy));

        /// <summary>
        /// CertificateHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public CertificateHttpHeaderAuthStrategy(string endPointUrl) : base(endPointUrl) { }
            
        /// <summary>
        /// Processing for TokenAuthorization using SignatureCredential
        /// </summary>
        /// <param name="certCredential"></param>
        /// <param name="toknAuthorization"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> ProcessTokenAuthorization(
                CertificateCredential certCredential, TokenAuthorization toknAuthorization)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {
                OAuthGenerator signGenerator = new OAuthGenerator(certCredential.UserName, certCredential.Password);
                signGenerator.setHTTPMethod(OAuthGenerator.HTTPMethod.POST);
                signGenerator.setToken(toknAuthorization.AccessToken);
                signGenerator.setTokenSecret(toknAuthorization.TokenSecret);
                string tokenTimeStamp = Timestamp;
                signGenerator.setTokenTimestamp(tokenTimeStamp);
                logger.Debug("token = " + toknAuthorization.AccessToken + " tokenSecret=" + toknAuthorization.TokenSecret + " uri=" + endpointURL);
                signGenerator.setRequestURI(endpointURL);

                //Compute Signature
                string sign = signGenerator.ComputeSignature();
                logger.Debug("Permissions signature: " + sign);
                string authorization = "token=" + toknAuthorization.AccessToken + ",signature=" + sign + ",timestamp=" + tokenTimeStamp;
                logger.Debug("Authorization string: " + authorization);
                headers.Add(BaseConstants.PAYPAL_AUTHORIZATION_PLATFORM_HEADER, authorization);
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
