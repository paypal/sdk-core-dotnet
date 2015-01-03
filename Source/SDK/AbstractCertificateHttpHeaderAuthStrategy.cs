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
        protected string endpointUrl;

        /// <summary>
        /// AbstractCertificateHttpHeaderAuthStrategy constructor
        /// </summary>
        /// <param name="endpointUrl"></param>
        protected AbstractCertificateHttpHeaderAuthStrategy(string endpointUrl)
        {
            this.endpointUrl = endpointUrl;
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
                    headers.Add(BaseConstants.PayPalSecurityUserIdHeader, credential.UserName);
                    headers.Add(BaseConstants.PayPalSecurityPasswordHeader, credential.Password);
                }
            }
            catch (OAuthException oex)
            {
                throw oex;
            }
            return headers;
        }

        /// <summary>
        ///  Process Token Authorization based on API format
        /// </summary>
        /// <param name="certCredential"></param>
        /// <param name="tokenAuthorize"></param>
        /// <returns></returns>
        protected abstract Dictionary<string, string> ProcessTokenAuthorization(CertificateCredential certCredential, TokenAuthorization tokenAuthorize);
    }
}
