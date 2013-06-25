using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal
{
    public abstract class AbstractCertificateHttpHeaderAuthStrategy : IAuthenticationStrategy<Dictionary<string, string>, CertificateCredential>
    {
        /// <summary>
        /// Endpoint url
        /// </summary>
        protected string endpointURL;

        /// <summary>
        /// AbstractCertificateHttpHeaderAuthStrategy constructor
        /// </summary>
        /// <param name="endpointUrl"></param>
        public AbstractCertificateHttpHeaderAuthStrategy(string endpointUrl)
        {
            this.endpointURL = endpointUrl;
        }

        /// <summary>
        /// Returns the Certificate Credential as HTTP headers
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public Dictionary<string, string> GenerateHeaderStrategy(CertificateCredential credential)
        {
            Dictionary<string, string> headers = null;

            try
            {
                if (credential.ThirdPartyAuthorization is TokenAuthorization)
                {
                    headers = ProcessTokenAuthorization(credential, (TokenAuthorization)credential.ThirdPartyAuthorization);
                }
                else
                {
                    headers = new Dictionary<string, string>();
                    headers.Add(BaseConstants.PAYPAL_SECURITY_USERID_HEADER, credential.UserName);
                    headers.Add(BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER, credential.Password);
                }
            }
            catch (OAuthException ae)
            {
                throw ae;
            }
            return headers;
        }

        /// <summary>
        /// Process Token Authorization based on API format
        /// </summary>
        /// <param name="certCredential"></param>
        /// <param name="toknAuthorization"></param>
        /// <returns></returns>
        protected abstract Dictionary<string, string> ProcessTokenAuthorization(CertificateCredential certCredential, TokenAuthorization toknAuthorization);
    }
}
