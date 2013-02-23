using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using PayPal.Authentication;
using PayPal.Exception;
using PayPal.Manager;

namespace PayPal.SOAP
{
    public class MerchantAPICallPreHandler : IAPICallPreHandler
    { 
        /// <summary>
	    /// API Username for authentication
	    /// </summary>
	    private string apiUserName;

	    /// <summary>
	    /// ICredential instance for authentication
	    /// </summary>
	    private ICredential credential;

	    /// <summary>
	    /// Access token if any for authorization
	    /// </summary>
	    private string accessToken;

	    /// <summary>
	    /// TokenSecret if any for authorization
	    /// </summary>
	    private string tokenSecret;

        /// <summary>
        /// API Password for authentication
        /// </summary>
        private string apiPassword;

        /// <summary>
        /// API Signature for authentication
        /// </summary>
        private string apiSignature;

        /// <summary>
        /// API Password for authentication
        /// </summary>
        private string applicationId;

        /// <summary>
        /// API Signature for authentication
        /// </summary>
        private string signatureSubject;

        /// <summary>
        /// API Signature for authentication
        /// </summary>
        private string endPoint;

	    /// <summary>
	    /// IAPICallPreHandler instance
	    /// </summary>
	    private IAPICallPreHandler apiCallHandler;

        /// <summary>
        /// SDK Name used in tracking
        /// </summary>
        private string sdkNme;

        /// <summary>
        /// SDK Version
        /// </summary>
        private string sdkVrsion;
        
	    /// <summary>
	    /// Internal variable to hold headers
	    /// </summary>
	    private Dictionary<string, string> headers;
        
	    /// <summary>
	    /// Internal variable to hold payload
	    /// </summary>
	    private string payLoad;

        /// <summary>
        /// Port name
        /// </summary>
        private string prtName;

        private readonly CredentialManager credentialMgr;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="apiCallHandler"></param>
        private MerchantAPICallPreHandler(CredentialManager credentialMgr, IAPICallPreHandler apiCallHandler) : base()
        {
            this.credentialMgr = credentialMgr;
            this.apiCallHandler = apiCallHandler;
        }  

        /// <summary>
        /// SOAPAPICallPreHandler decorating basic IAPICallPreHandler using API Username, optional API Password, optional API Signature, optional applicationId, optional signatureSubject
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <param name="apiUserName"></param>
        /// <param name="accessToken"></param>
        /// <param name="tokenSecret"></param>
        /// <param name="apiPassword"></param>
        /// <param name="apiSignature"></param>
        public MerchantAPICallPreHandler(IConfigManager configMgr, CredentialManager credentialMgr, IAPICallPreHandler apiCallHandler, string apiUserName, string accessToken, string tokenSecret)
            : this(credentialMgr, apiCallHandler)
		{
            try
            {
                this.credentialMgr = credentialMgr;
                this.apiUserName = apiUserName;
                this.accessToken = accessToken;
                this.tokenSecret = tokenSecret;
                this.endPoint = configMgr.GetProperty("endpoint");
                this.apiPassword = configMgr.GetProperty("apiPassword");
                this.apiSignature = configMgr.GetProperty("apiSignature");
                this.applicationId = configMgr.GetProperty("applicationId");
                this.signatureSubject = configMgr.GetProperty("signatureSubject");
                InitCredential();
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
	    }

	    /// <summary>
	    ///  SOAPAPICallPreHandler decorating basic IAPICallPreHandler using ICredential
	    /// </summary>
	    /// <param name="apiCallHandler"></param>
	    /// <param name="credential"></param>
	    public MerchantAPICallPreHandler(CredentialManager credentialMgr, IAPICallPreHandler apiCallHandler, ICredential credential) : this(credentialMgr, apiCallHandler)
	    {
	        this.credentialMgr = credentialMgr;
		    if (credential == null) 
            {
			    throw new ArgumentException("Credential is null in SOAPAPICallPreHandler");
		    }
		    this.credential = credential;
	    }
        
        /// <summary>
        /// Gets and sets the SDK Name
        /// </summary>
        public string SDKName
        {
            get
            {
                return sdkNme;
            }
            set
            {
                this.sdkNme = value;
            }
        }

        /// <summary>
        /// Gets and sets the SDK version
        /// </summary>
        public string SDKVersion
        {
            get
            {
                return sdkVrsion;
            }
            set
            {
                this.sdkVrsion = value;
            }
        }

        /// <summary>
        /// Gets and sets the port mame
        /// </summary>
        public string PortName
        {
            get
            {
                return prtName;
            }
            set
            {
                this.prtName = value;
            }
        }

        /// <summary>
        /// Returns the Header
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetHeaderMap()
        {
            try
            {
                if (headers == null)
                {
                    headers = apiCallHandler.GetHeaderMap();
                    if (credential is SignatureCredential)
                    {
                        SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(GetEndPoint());
                        headers = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy((SignatureCredential)credential);
                    }
                    else if (credential is CertificateCredential)
                    {
                        CertificateHttpHeaderAuthStrategy certificateHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(GetEndPoint());
                        headers = certificateHttpHeaderAuthStrategy.GenerateHeaderStrategy((CertificateCredential)credential);
                    }

                    foreach (KeyValuePair<string, string> pair in GetDefaultHttpHeadersSOAP())
                    {
                        headers.Add(pair.Key, pair.Value);
                    }
                }
            }
            catch (OAuthException ae)
            {
                throw ae;
            }
            return headers;
        }	    
        
        /// <summary>
        /// Appends SOAP Headers to payload 
        /// if the credentials mandate soap headers
        /// </summary>
        /// <returns></returns>
	    public string GetPayLoad() 
        {
		    if (payLoad == null) 
            {
                payLoad = apiCallHandler.GetPayLoad();
			    string header = null;
			    if (credential is SignatureCredential)
                {
				    SignatureCredential signCredential = (SignatureCredential) credential;
				    SignatureSOAPHeaderAuthStrategy signSoapHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
				    signSoapHeaderAuthStrategy.ThirdPartyAuthorization = signCredential.ThirdPartyAuthorization;						    
				    header = signSoapHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
			    } 
                else if (credential is CertificateCredential) 
                {
				    CertificateCredential certCredential = (CertificateCredential) credential;
				    CertificateSOAPHeaderAuthStrategy certSoapHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
				    certSoapHeaderAuthStrategy.ThirdPartyAuthorization = certCredential.ThirdPartyAuthorization;					
				    header = certSoapHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);

			    }
			    payLoad = GetPayLoadUsingSOAPHeader(payLoad, GetAttributeNamespace(), header);
		    }
		    return payLoad;
	    }

        /// <summary>
        /// Returns the endpoint url
        /// </summary>
        /// <returns></returns>
	    public string GetEndPoint()
        {
            return this.endPoint;
        }
        
        /// <summary>
        /// Returns the instance of ICredential
        /// </summary>
        /// <returns></returns>
	    public ICredential GetCredential() 
        {
		    return credential;
	    }

        /// <summary>
        /// Returns the credentials as configured in the application configuration
        /// </summary>
        /// <returns></returns>
        private ICredential GetCredentials()
        {
            ICredential returnCredential = null;
            try
            {
                returnCredential = credentialMgr.GetCredentials(apiUserName);

                if (!string.IsNullOrEmpty(accessToken))
                {

                    // Set third party authorization to token
                    // if token is sent as part of request call
                    IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(accessToken, tokenSecret);
                    if (returnCredential is SignatureCredential)
                    {
                        SignatureCredential signCredential = (SignatureCredential)returnCredential;
                        signCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
                    }
                    else if (returnCredential is CertificateCredential)
                    {
                        CertificateCredential certCredential = (CertificateCredential)returnCredential;
                        certCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return returnCredential;
        }

	    /// <summary>
        /// Returns default HTTP headers used in SOAP call
	    /// </summary>
	    /// <returns></returns>
	    private Dictionary<string, string> GetDefaultHttpHeadersSOAP() 
        {
		    Dictionary<string, string> returnMap = new Dictionary<string, string>();
		    returnMap.Add(BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER, BaseConstants.SOAP);
            returnMap.Add(BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER, BaseConstants.SOAP);
            returnMap.Add(BaseConstants.PAYPAL_REQUEST_SOURCE_HEADER, SDKName + "-" + SDKVersion);
		    return returnMap;
	    }        

        /// <summary>
        /// Initializes the instance of ICredential
        /// </summary>
	    private void InitCredential()  
        {
            try
            {
                if (credential == null)
                {
                    credential = GetCredentials();
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
	    }

	    /// <summary>
        /// Returns Namespace specific to PayPal APIs
	    /// </summary>
	    /// <returns></returns>
        private string GetAttributeNamespace() 
        {
		    string AttributeNamespace = "xmlns:ns=\"urn:ebay:api:PayPalAPI\" xmlns:ebl=\"urn:ebay:apis:eBLBaseComponents\" xmlns:cc=\"urn:ebay:apis:CoreComponentTypes\" xmlns:ed=\"urn:ebay:apis:EnhancedDataTypes\"";
            return AttributeNamespace;
	    }

	    /// <summary>
        /// Returns Payload after decoration
	    /// </summary>
	    /// <param name="payLoad"></param>
	    /// <param name="namespaces"></param>
	    /// <param name="header"></param>
	    /// <returns></returns>
	    private string GetPayLoadUsingSOAPHeader(string payLoad, string namespaces, string header) 
        {
            string returnPayLoad = null;
		    string formattedPayLoad = payLoad;
		    returnPayLoad = string.Format(formattedPayLoad, new object[] {namespaces, header});
		    return returnPayLoad;
	    }
    }
}
