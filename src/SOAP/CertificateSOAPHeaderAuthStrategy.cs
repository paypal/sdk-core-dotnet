using System.Text;
using PayPal.Authentication;

namespace PayPal.SOAP
{
    public class CertificateSOAPHeaderAuthStrategy : IAuthenticationStrategy<string, CertificateCredential>
    {
        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public CertificateSOAPHeaderAuthStrategy() { }

        /// <summary>
        /// Gets and sets any instance of {@link ThirdPartyAuthorization}
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the Header
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public string GenerateHeaderStrategy(CertificateCredential credential) 
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

        /// <summary>
        /// Returns an empty SOAP Header String
        /// Token authorization does not bear a credential part
        /// </summary>
        /// <returns></returns>
        private string TokenAuthPayLoad()
        {
            StringBuilder soapMessage = new StringBuilder();
            soapMessage.Append("<ns:RequesterCredentials/>");
            return soapMessage.ToString();
        }

        private string AuthPayLoad(CertificateCredential credential,
                SubjectAuthorization subjectAuthorization)
        {
            StringBuilder soapMessage = new StringBuilder();
            soapMessage.Append("<ns:RequesterCredentials>");
            soapMessage.Append("<ebl:Credentials>");
            soapMessage.Append("<ebl:Username>" + credential.UserName
                    + "</ebl:Username>");
            soapMessage.Append("<ebl:Password>" + credential.Password
                    + "</ebl:Password>");

            // Append subject credential if available
            if (subjectAuthorization != null)
            {
                soapMessage.Append("<ebl:Subject>" + subjectAuthorization.Subject
                        + "</ebl:Subject>");
            }
            soapMessage.Append("</ebl:Credentials>");
            soapMessage.Append("</ns:RequesterCredentials>");
            return soapMessage.ToString();
        }
    }
}
