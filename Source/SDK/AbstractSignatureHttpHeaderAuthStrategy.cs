using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal
{
    public abstract class AbstractSignatureHttpHeaderAuthStrategy : IAuthenticationStrategy<Dictionary<string, string>, SignatureCredential>
    {
        /// <summary>
        /// Endpoint url
        /// </summary>
        protected string endpointUrl;

        /// <summary>
        /// AbstractCertificateHttpHeaderAuthStrategy constructor
        /// </summary>
        /// <param name="endpointUrl"></param>
        protected AbstractSignatureHttpHeaderAuthStrategy(string endpointUrl)
        {
            this.endpointUrl = endpointUrl;
        }

        /// <summary>
        /// Returns CertificateCredential as HTTP headers
        /// </summary>
        /// <param name="signCredential"></param>
        /// <returns></returns>
        public Dictionary<string, string> GenerateHeaderStrategy(SignatureCredential signCredential)
        {
            Dictionary<string, string> headers = null;

            try
            {
                if (signCredential.ThirdPartyAuthorization is TokenAuthorization)
                {
                    headers = ProcessTokenAuthorization(signCredential,(TokenAuthorization)signCredential.ThirdPartyAuthorization);
                }
                else
                {
                    headers = new Dictionary<string, string>();
                    headers.Add(BaseConstants.PayPalSecurityUserIdHeader, signCredential.UserName);
                    headers.Add(BaseConstants.PayPalSecurityPasswordHeader,signCredential.Password);
                    headers.Add(BaseConstants.PayPalSecuritySignatureHeader,signCredential.Signature);
                }
            }
            catch (OAuthException oex)
            {
                throw oex;
            }
            return headers;
        }

        /// <summary>
        /// Process Token Authorization based on API format
        /// </summary>
        /// <param name="signCredential"></param>
        /// <param name="tokenAuthorize"></param>
        /// <returns></returns>
        protected internal abstract Dictionary<string, string> ProcessTokenAuthorization(SignatureCredential signCredential, TokenAuthorization tokenAuthorize);
    }
}
