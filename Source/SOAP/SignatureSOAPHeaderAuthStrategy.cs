using System.Text;
using PayPal.Authentication;

namespace PayPal.SOAP
{
    public class SignatureSOAPHeaderAuthStrategy : IAuthenticationStrategy<string, SignatureCredential>
    {
        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public SignatureSOAPHeaderAuthStrategy() { }

        /// <summary>
        /// Third Party Authorization
        /// </summary>
        private IThirdPartyAuthorization authorization;

        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get
            {
                return this.authorization;
            }
            set
            {
                this.authorization = value;
            }
        }

        public string GenerateHeaderStrategy(SignatureCredential credential)
        {
            string payload = null;
            if (ThirdPartyAuthorization is TokenAuthorization)
            {
                payload = TokenAuthPayload();
            }
            else if (ThirdPartyAuthorization is SubjectAuthorization)
            {
                payload = AuthPayload(credential, (SubjectAuthorization)ThirdPartyAuthorization);
            }
            else
            {
                payload = AuthPayload(credential, null);
            }
            return payload;
        }

        private string TokenAuthPayload()
        {
            StringBuilder soapMessage = new StringBuilder();
            soapMessage.Append("<ns:RequesterCredentials/>");
            return soapMessage.ToString();
        }

        private string AuthPayload(SignatureCredential signCredential,
                SubjectAuthorization subjectAuth)
        {  
            StringBuilder soapMessage = new StringBuilder();
            soapMessage.Append("<ns:RequesterCredentials>");
            soapMessage.Append("<ebl:Credentials>");
            soapMessage.Append("<ebl:Username>" + signCredential.UserName
                    + "</ebl:Username>");
            soapMessage.Append("<ebl:Password>" + signCredential.Password
                    + "</ebl:Password>");
            soapMessage.Append("<ebl:Signature>" + signCredential.Signature
                    + "</ebl:Signature>");
            if (subjectAuth != null)
            {
                soapMessage.Append("<ebl:Subject>" + subjectAuth.Subject
                        + "</ebl:Subject>");
            }
            soapMessage.Append("</ebl:Credentials>");
            soapMessage.Append("</ns:RequesterCredentials>");
            return soapMessage.ToString();
        }
    }
}
