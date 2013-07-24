using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;
using PayPal.Log;

namespace PayPal.SOAP
{
    public class CertificateHttpHeaderAuthStrategy : AbstractCertificateHttpHeaderAuthStrategy
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger(typeof(CertificateHttpHeaderAuthStrategy));

        /// <summary>
        /// CertificateHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public CertificateHttpHeaderAuthStrategy(string endPointUrl) : base(endPointUrl) { }

        /// <summary>
        /// Processing for TokenAuthorization} using SignatureCredential
        /// </summary>
        /// <param name="certCredential"></param>
        /// <param name="tokenAuthorize"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> ProcessTokenAuthorization(CertificateCredential certCredential, TokenAuthorization tokenAuthorize)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {
                OAuthGenerator signGenerator = new OAuthGenerator(certCredential.UserName, certCredential.Password);
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
