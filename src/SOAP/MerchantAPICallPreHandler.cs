using System;
using System.Collections.Generic;
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
	    private string APIUserName;

	    /// <summary>
	    /// ICredential instance for authentication
	    /// </summary>
	    private ICredential Credential;

	    /// <summary>
	    /// Access token if any for authorization
	    /// </summary>
	    private string AccessToken;

	    /// <summary>
	    /// TokenSecret if any for authorization
	    /// </summary>
	    private string TokenSecret;

	    /// <summary>
	    /// IAPICallPreHandler instance
	    /// </summary>
	    private IAPICallPreHandler APICallHandler;
       
	    /// <summary>
	    /// Internal variable to hold headers
	    /// </summary>
	    private Dictionary<string, string> Headers;
        
	    /// <summary>
	    /// Internal variable to hold payload
	    /// </summary>
	    private string PayLoad;

        /// <summary>
        /// SDK Configuration
        /// </summary>
        private Dictionary<string, string> Config;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="apiCallHandler"></param>
        private MerchantAPICallPreHandler(IAPICallPreHandler apiCallHandler, Dictionary<string, string> config)
            : base()
        {
            this.APICallHandler = apiCallHandler;
            this.Config = (config == null) ? ConfigManager.Instance.GetProperties() : config;
        }  

        /// <summary>
        /// SOAPAPICallPreHandler decorating basic IAPICallPreHandler using API Username
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <param name="apiUserName"></param>
        /// <param name="accessToken"></param>
        /// <param name="tokenSecret"></param>
	    public MerchantAPICallPreHandler(Dictionary<string, string> config, IAPICallPreHandler apiCallHandler, string apiUserName, string accessToken, string tokenSecret) : this(apiCallHandler, config)
		{
            try
            {
                this.APIUserName = apiUserName;
                this.AccessToken = accessToken;
                this.TokenSecret = tokenSecret;
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
        public MerchantAPICallPreHandler(Dictionary<string, string> config, IAPICallPreHandler apiCallHandler, ICredential credential) : this(apiCallHandler, config)
        {	    
		    if (credential == null) 
            {
			    throw new ArgumentException("Credential is null in SOAPAPICallPreHandler");
		    }
		    this.Credential = credential;
	    }

#if NET_2_0
        /// <summary>
        /// SDK Name
        /// </summary>
        private string Name;

        /// <summary>
        /// Gets and sets the SDK Name
        /// </summary>
        public string SDKName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// SDK Version
        /// </summary>
        private string Version;

        /// <summary>
        /// Gets and sets the SDK Version
        /// </summary>
        public string SDKVersion
        {
            get
            {
                return this.Version;
            }
            set
            {
                this.Version = value;
            }
        }

        /// <summary>
        /// Port Name
        /// </summary>
        private string Port;

        /// <summary>
        /// Gets and sets the Port Name
        /// </summary>
        public string PortName
        {
            get
            {
                return this.Port;

            }
            set
            {
                this.Port = value;
            }
        }
#else
        /// <summary>
        /// Gets and sets the SDK Name
        /// </summary>
        public string SDKName
        {
            get;
            set;
        }

	    /// <summary>
        /// Gets and sets the SDK Version
	    /// </summary>
        public string SDKVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the Port Name
        /// </summary>
        public string PortName
        {
            get;
            set;
        }
#endif
        /// <summary>
        /// Returns the Header
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetHeaderMap()
        {
            try
            {
                if (Headers == null)
                {
                    Headers = APICallHandler.GetHeaderMap();
                    if (Credential is SignatureCredential)
                    {
                        SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(GetEndPoint());
                        Headers = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy((SignatureCredential)Credential);
                    }
                    else if (Credential is CertificateCredential)
                    {
                        CertificateHttpHeaderAuthStrategy certificateHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(GetEndPoint());
                        Headers = certificateHttpHeaderAuthStrategy.GenerateHeaderStrategy((CertificateCredential)Credential);
                    }

                    foreach (KeyValuePair<string, string> pair in GetDefaultHttpHeadersSOAP())
                    {
                        Headers.Add(pair.Key, pair.Value);
                    }
                }
            }
            catch (OAuthException ae)
            {
                throw ae;
            }
            return Headers;
        }	    
        
        /// <summary>
        /// Appends SOAP Headers to payload 
        /// if the credentials mandate soap headers
        /// </summary>
        /// <returns></returns>
	    public string GetPayLoad() 
        {
		    if (PayLoad == null) 
            {
                PayLoad = APICallHandler.GetPayLoad();
			    string header = null;
			    if (Credential is SignatureCredential)
                {
				    SignatureCredential signCredential = (SignatureCredential) Credential;
				    SignatureSOAPHeaderAuthStrategy signSoapHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
				    signSoapHeaderAuthStrategy.ThirdPartyAuthorization = signCredential.ThirdPartyAuthorization;						    
				    header = signSoapHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
			    } 
                else if (Credential is CertificateCredential) 
                {
				    CertificateCredential certCredential = (CertificateCredential) Credential;
				    CertificateSOAPHeaderAuthStrategy certSoapHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
				    certSoapHeaderAuthStrategy.ThirdPartyAuthorization = certCredential.ThirdPartyAuthorization;					
				    header = certSoapHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);

			    }
			    PayLoad = GetPayLoadUsingSOAPHeader(PayLoad, GetAttributeNamespace(), header);
		    }
		    return PayLoad;
	    }

        /// <summary>
        /// Returns the endpoint url
        /// </summary>
        /// <returns></returns>
	    public string GetEndPoint() 
        {
            string endpoint = null;
            if (PortName != null && Config.ContainsKey(PortName) && !string.IsNullOrEmpty(Config[PortName]))
            {
                endpoint = Config[PortName];
            }
            else if (Config.ContainsKey(BaseConstants.EndpointConfig))
            {
                endpoint = APICallHandler.GetEndPoint();
            }
            else if (Config.ContainsKey(BaseConstants.ApplicationModeConfig))
            {
                switch (Config[BaseConstants.ApplicationModeConfig].ToLower())
                {
                    case BaseConstants.LiveMode:
                        if (Credential is SignatureCredential)
                        {
                            endpoint = BaseConstants.MerchantSignatureLiveEndpoint;
                        }
                        else if (Credential is CertificateCredential)
                        {
                            endpoint = BaseConstants.MerchantCertificateLiveEndpoint;
                        }
                        break;
                    case BaseConstants.SandboxMode:
                        if (Credential is SignatureCredential)
                        {
                            endpoint = BaseConstants.MerchantSignatureSandboxEndpoint;
                        }
                        else if (Credential is CertificateCredential)
                        {
                            endpoint = BaseConstants.MerchantCertificateSandboxEndpoint;
                        }
                        break;
                    default:
                        throw new ConfigException("You must specify one of mode(live/sandbox) OR endpoint in the configuration");
                }
            }
            else
            {
                throw new ConfigException("You must specify one of mode(live/sandbox) OR endpoint in the configuration");
            }
            return endpoint;
	    }
        
        /// <summary>
        /// Returns the instance of ICredential
        /// </summary>
        /// <returns></returns>
	    public ICredential GetCredential() 
        {
		    return Credential;
	    } 

        /// <summary>
        ///  Returns the credentials as configured in the application configuration
        /// </summary>
        /// <returns></returns>
	    private ICredential GetCredentials() 
        {
            ICredential returnCredential = null;
            try
            {                
                CredentialManager credentialMngr = CredentialManager.Instance;
                returnCredential = credentialMngr.GetCredentials(this.Config, APIUserName);

                if (!string.IsNullOrEmpty(AccessToken))
                {

                    // Set third party authorization to token
                    // if token is sent as part of request call
                    IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(AccessToken, TokenSecret);
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
            catch(System.Exception ex)
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
		    returnMap.Add(BaseConstants.PayPalRequestDataFormatHeader, BaseConstants.SOAP);
            returnMap.Add(BaseConstants.PayPalResponseDataFormatHeader, BaseConstants.SOAP);
            returnMap.Add(BaseConstants.PayPalRequestSourceHeader, SDKName + "-" + SDKVersion);
		    return returnMap;
	    }        

        /// <summary>
        /// Initializes the instance of ICredential
        /// </summary>
	    private void InitCredential()  
        {
            try
            {
                if (Credential == null)
                {
                    Credential = GetCredentials();
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
            Regex regex = new Regex("\\{(?![01]})");
            string formattedPayLoad = regex.Replace(payLoad, "{{");
            regex = new Regex("(?<!\\{[01]{1})}");
            formattedPayLoad = regex.Replace(formattedPayLoad, "}}");
            returnPayLoad = string.Format(formattedPayLoad, new object[] { namespaces, header });
            return returnPayLoad;
	    }
    }
}
