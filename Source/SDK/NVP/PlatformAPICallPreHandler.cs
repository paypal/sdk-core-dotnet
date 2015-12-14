using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Manager;
using PayPal.Exception;
using PayPal.Util;

namespace PayPal.NVP
{
    public class PlatformAPICallPreHandler : IAPICallPreHandler
    {
        /// <summary>
        /// Service Name
        /// </summary>
	    private readonly string serviceName;

        /// <summary>
        /// API method
        /// </summary>
	    private readonly string apiMethod;

        /// <summary>
        /// Raw payload from stubs
        /// </summary>
		private readonly string rawPayload;

	    /// <summary>
	    /// API Username for authentication
	    /// </summary>
	    private string apiUserName;

	    /// <summary>
	    /// {@link ICredential} for authentication
	    /// </summary>
	    private ICredential credential;
        
        /// <summary>
        /// Access token if any for authorization
        /// </summary>
		private string accessToken;
        
        /// <summary>
        /// Access token secret if any for authorization
        /// </summary>
        private string accessTokenSecret;
        	          
        /// <summary>
        /// Internal variable to hold headers
        /// </summary>
	    private Dictionary<string, string> headers;
               
        /// <summary>
        /// SDK Configuration
        /// </summary>
        private Dictionary<string, string> config;
        
        /// <summary>
        /// SDK Name
        /// </summary>
        private string nameSDK;

        /// <summary>
        /// SDK Version
        /// </summary>
        private string versionSDK;

        /// <summary>
        /// Port Name
        /// </summary>
        private string namePort;

        /// <summary>
	    /// Private constructor
	    /// </summary>
	    /// <param name="rawPayoad"></param>
	    /// <param name="serviceName"></param>
	    /// <param name="method"></param>
        private PlatformAPICallPreHandler(string rawPayoad, string serviceName, string method, Dictionary<string, string> config)
            : base()
        {
            this.rawPayload = rawPayoad;
		    this.serviceName = serviceName;
		    this.apiMethod = method;
            this.config = (config == null) ? ConfigManager.Instance.GetProperties() : config;
	    }

        /// <summary>
        /// NVPAPICallPreHandler
        /// </summary>
        /// <param name="rawPayload"></param>
        /// <param name="serviceName"></param>
        /// <param name="method"></param>
        /// <param name="apiUserName"></param>
        /// <param name="accessToken"></param>
        /// <param name="accesstokenSecret"></param>
	    public PlatformAPICallPreHandler(Dictionary<string, string> config, string rawPayload, string serviceName, string method,
            string apiUserName, string accessToken, string accesstokenSecret)
            : this(rawPayload, serviceName, method, config)
        {
            try
            {
                this.apiUserName = apiUserName;
                this.accessToken = accessToken;
                this.accessTokenSecret = accesstokenSecret;
                InitCredential();
            }
            catch(System.Exception ex)
            {
                throw ex;
            }		    
	    }

	    /// <summary>
        /// NVPAPICallPreHandler
	    /// </summary>
	    /// <param name="rawPayload"></param>
	    /// <param name="serviceName"></param>
	    /// <param name="method"></param>
	    /// <param name="credential"></param>
	    public PlatformAPICallPreHandler(Dictionary<string, string> config, string rawPayload, string serviceName,string method,
            ICredential credential)
            : this(rawPayload, serviceName, method, config)
        {  		
		    if (credential == null) 
            {
			    throw new ArgumentException("Credential is null in NVPAPICallPreHandler");
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
                return this.nameSDK;
            }
            set
            {
                this.nameSDK = value;
            }
        }

        /// <summary>
        /// Gets and sets the SDK Version
        /// </summary>
        public string SDKVersion
        {
            get
            {
                return this.versionSDK;
            }
            set
            {
                this.versionSDK = value;
            }
        }

        /// <summary>
        /// Gets and sets the Port Name
        /// </summary>
        public string PortName
        {
            get
            {
                return this.namePort;

            }
            set
            {
                this.namePort = value;
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
                    headers = new Dictionary<string, string>();
                    if (credential is SignatureCredential)
                    {
                        SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(GetEndpoint());
                        headers = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy((SignatureCredential)credential);
                    }
                    else if (credential is CertificateCredential)
                    {
                        CertificateHttpHeaderAuthStrategy certificateHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(GetEndpoint());
                        headers = certificateHttpHeaderAuthStrategy.GenerateHeaderStrategy((CertificateCredential)credential);
                    }
                    foreach (KeyValuePair<string, string> pair in GetDefaultHttpHeadersNVP())
                    {
                        headers.Add(pair.Key, pair.Value);
                    }
                }
            }
            catch (OAuthException oex)
            {
                throw oex;
            }
            return headers;
        }

        /// <summary>
        /// Returns the raw payload as no processing necessary for NVP
        /// </summary>
        /// <returns></returns>
	    public string GetPayload() 
        {
		    return rawPayload;
	    }

        /// <summary>
        /// Returns the endpoint url
        /// </summary>
        /// <returns></returns>
	    public string GetEndpoint()
        {
            string endpoint = null;
            if (PortName != null && config.ContainsKey(PortName) && !string.IsNullOrEmpty(config[PortName]))
            {
                endpoint = config[PortName];
            }
            else if (config.ContainsKey(BaseConstants.EndpointConfig))
            {
                endpoint = config[BaseConstants.EndpointConfig];
            }
            else if (config.ContainsKey(BaseConstants.ApplicationModeConfig))
            {
                switch (config[BaseConstants.ApplicationModeConfig].ToLower())
                {
                    case BaseConstants.LiveMode:
                        endpoint = BaseConstants.PlatformLiveEndpoint;
                        break;
                    case BaseConstants.SandboxMode:
                        endpoint = BaseConstants.PlatformSandboxEndpoint;
                        break;
                    case BaseConstants.TestSandboxMode:
                        endpoint = BaseConstants.PlatformTestSandboxEndpoint;
                        break;
                    default:
                        throw new ConfigException("You must specify one of mode(live/sandbox) OR endpoint in the configuration");
                }                
            }
            else
            {
                throw new ConfigException("You must specify one of mode or endpoint in the configuration");
            }
            
            if (endpoint != null)
            {
                if(!endpoint.EndsWith("/"))
                {
                    endpoint = endpoint + "/";
                }
                endpoint = endpoint + serviceName + "/" + apiMethod;
            }
            return endpoint;
        }

        /// <summary>
        /// Reurns instance of ICredential
        /// </summary>
        /// <returns></returns>
	    public ICredential GetCredential() 
        {
		    return credential;
	    }

        /// <summary>
        /// Returns the credentials
        /// </summary>
        /// <returns></returns>
	    private ICredential GetCredentials()  
        {
		    ICredential returnCredential = null;

            try
            {
                CredentialManager credentialMngr = CredentialManager.Instance;
                returnCredential = credentialMngr.GetCredentials(this.config, apiUserName);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    IThirdPartyAuthorization tokenAuthuthorize = new TokenAuthorization(accessToken, accessTokenSecret);

                    if (returnCredential is SignatureCredential)
                    {
                        SignatureCredential sigCred = (SignatureCredential)returnCredential;
                        sigCred.ThirdPartyAuthorization = tokenAuthuthorize;
                    }
                    else if (returnCredential is CertificateCredential)
                    {
                        CertificateCredential certCred = (CertificateCredential)returnCredential;
                        certCred.ThirdPartyAuthorization = tokenAuthuthorize;
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
        /// Returns the Default Http Headers NVP
        /// </summary>
        /// <returns></returns>
	    private Dictionary<string, string> GetDefaultHttpHeadersNVP() 
        {
		    Dictionary<string, string> returnMap = new Dictionary<string, string>();

            try
            {
                returnMap.Add(BaseConstants.PayPalApplicationIdHeader, GetApplicationId());
                returnMap.Add(BaseConstants.PayPalRequestDataFormatHeader, BaseConstants.NVP);
                returnMap.Add(BaseConstants.PayPalResponseDataFormatHeader, BaseConstants.NVP);
                returnMap.Add(BaseConstants.PayPalRequestSourceHeader, SDKName + "-" + SDKVersion);
                returnMap.Add(BaseConstants.PayPalSandboxEmailAddressHeader, GetSandboxEmailAddress());
                returnMap.Add(BaseConstants.PayPalSandboxDeviceIPAddress, GetDeviceIPAddress());
                SDKUtil.AddUserAgentToHeader(returnMap, SDKName, SDKVersion);
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
		    return returnMap;
	    }

        /// <summary>
        /// Returns Application Id
        /// </summary>
        /// <returns></returns>
	    private string GetApplicationId() 
        {
		    string applicationId = string.Empty;
		    if (credential is CertificateCredential) 
            {
			    applicationId = ((CertificateCredential) credential).ApplicationId;
		    } 
            else if (credential is SignatureCredential) 
            {
			    applicationId = ((SignatureCredential) credential).ApplicationId;
		    }
		    return applicationId;
	    }

	    private void InitCredential() 
        {
		    if (credential == null) 
            {
                try
                {
                    credential = GetCredentials();
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }                
		    }
	    }

        private string GetDeviceIPAddress()
        {
            if (config.ContainsKey(BaseConstants.ClientIPAddressConfig) && 
                !string.IsNullOrEmpty(config[BaseConstants.ClientIPAddressConfig]))
            {
                return config[BaseConstants.ClientIPAddressConfig];
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetSandboxEmailAddress()
        {
            if (config.ContainsKey(BaseConstants.PayPalSandboxEmailAddressConfig) && 
                !string.IsNullOrEmpty(config[BaseConstants.PayPalSandboxEmailAddressConfig]))
            {
                return config[BaseConstants.PayPalSandboxEmailAddressConfig];
            }
            else
            {
                return BaseConstants.PayPalSandboxEmailAddressDefault;
            }
        }    
    }
}
