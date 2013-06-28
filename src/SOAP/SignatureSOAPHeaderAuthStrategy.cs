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

#if NET_2_0
        /// <summary>
        /// Third Party Authorization
        /// </summary>
        private IThirdPartyAuthorization Authorization;

        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get
            {
                return this.Authorization;
            }
            set
            {
                this.Authorization = value;
            }
        }
#else
        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get;
            set;
        }
#endif

        public string GenerateHeaderStrategy(SignatureCredential credential)
        {
            string payLoad = null;
            if (ThirdPartyAuthorization is TokenAuthorization)
            {
                payLoad = TokenAuthPayLoad();
            }
            else if (ThirdPartyAuthorization is SubjectAuthorization)
            {
                payLoad = AuthPayLoad(credential, (SubjectAuthorization)ThirdPartyAuthorization);
            }
            else
            {
                payLoad = AuthPayLoad(credential, null);
            }
            return payLoad;
        }

        private string TokenAuthPayLoad()
        {
            StringBuilder soapMessage = new StringBuilder();
            soapMessage.Append("<ns:RequesterCredentials/>");
            return soapMessage.ToString();
        }

        private string AuthPayLoad(SignatureCredential signCredential,
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
