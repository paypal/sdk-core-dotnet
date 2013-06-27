using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal.NVP
{
    public class PlatformAPICallPreHandler : IAPICallPreHandler
    {
        /// <summary>
        /// Service Name
        /// </summary>
	    private readonly string ServiceName;

        /// <summary>
        /// API method
        /// </summary>
	    private readonly string Method;

        /// <summary>
        /// Raw payload from stubs
        /// </summary>
		private readonly string RawPayLoad;

	    /// <summary>
	    /// API Username for authentication
	    /// </summary>
	    private string APIUserName;

	    /// <summary>
	    /// {@link ICredential} for authentication
	    /// </summary>
	    private ICredential Credential;
        
        /// <summary>
        /// Access token if any for authorization
        /// </summary>
		private string AccessToken;
        
        /// <summary>
        /// Access token secret if any for authorization
        /// </summary>
        private string AccessTokenSecret;
        	          
        /// <summary>
        /// Internal variable to hold headers
        /// </summary>
	    private Dictionary<string, string> Headers;
               
        /// <summary>
        /// SDK Configuration
        /// </summary>
        private Dictionary<string, string> Config;

        /// <summary>
	    /// Private constructor
	    /// </summary>
	    /// <param name="rawPayLoad"></param>
	    /// <param name="serviceName"></param>
	    /// <param name="method"></param>
        private PlatformAPICallPreHandler(string rawPayLoad, string serviceName, string method, Dictionary<string, string> config)
            : base()
        {
            this.RawPayLoad = rawPayLoad;
		    this.ServiceName = serviceName;
		    this.Method = method;
            this.Config = (config == null) ? ConfigManager.Instance.GetProperties() : config;
	    }

        /// <summary>
        /// NVPAPICallPreHandler
        /// </summary>
        /// <param name="rawPayLoad"></param>
        /// <param name="serviceName"></param>
        /// <param name="method"></param>
        /// <param name="apiUserName"></param>
        /// <param name="accessToken"></param>
        /// <param name="tokenSecret"></param>
	    public PlatformAPICallPreHandler(Dictionary<string, string> config, string rawPayLoad, string serviceName, string method,
            string apiUserName, string accessToken, string tokenSecret)
            : this(rawPayLoad, serviceName, method, config)
        {
            try
            {
                this.APIUserName = apiUserName;
                this.AccessToken = accessToken;
                this.AccessTokenSecret = tokenSecret;
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
	    /// <param name="rawPayLoad"></param>
	    /// <param name="serviceName"></param>
	    /// <param name="method"></param>
	    /// <param name="credential"></param>
	    public PlatformAPICallPreHandler(Dictionary<string, string> config, string rawPayLoad, string serviceName,string method,
            ICredential credential)
            : this(rawPayLoad, serviceName, method, config)
        {  		
		    if (credential == null) 
            {
			    throw new ArgumentException("Credential is null in NVPAPICallPreHandler");
		    }
		    this.Credential = credential;
	    }
        	    
        /// <summary>
        /// Gets and sets the SDK Name
        /// </summary>
        public string SDKName
        {
            get;
            set;
        }

	    /// <summary>
        /// Gets and sets the SDK version
	    /// </summary>
        public string SDKVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the port name
        /// </summary>
        public string PortName
        {
            get;
            set;
        }

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
                    Headers = new Dictionary<string, string>();
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
                    foreach (KeyValuePair<string, string> pair in GetDefaultHttpHeadersNVP())
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
        /// Returns the raw payload as no processing necessary for NVP
        /// </summary>
        /// <returns></returns>
	    public string GetPayLoad() 
        {
		    return RawPayLoad;
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
                endpoint = Config[BaseConstants.EndpointConfig];
            }
            else if (Config.ContainsKey(BaseConstants.ApplicationModeConfig))
            {
                switch (Config[BaseConstants.ApplicationModeConfig].ToLower())
                {
                    case BaseConstants.LiveMode:
                        endpoint = BaseConstants.PlatformLiveEndPoint;
                        break;
                    case BaseConstants.SandboxMode:
                        endpoint = BaseConstants.PlatformSandboxEndpoint;
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
                endpoint = endpoint + ServiceName + "/" + Method;
            }
            return endpoint;
        }

        /// <summary>
        /// Reurns instance of ICredential
        /// </summary>
        /// <returns></returns>
	    public ICredential GetCredential() 
        {
		    return Credential;
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
                returnCredential = credentialMngr.GetCredentials(this.Config, APIUserName);

                if (!string.IsNullOrEmpty(AccessToken))
                {
                    IThirdPartyAuthorization tokenAuthuthorize = new TokenAuthorization(AccessToken, AccessTokenSecret);

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
                returnMap.Add(BaseConstants.PayPalApplicationIDHeader, GetApplicationID());
                returnMap.Add(BaseConstants.PayPalRequestDataFormatHeader, BaseConstants.NVP);
                returnMap.Add(BaseConstants.PayPalResponseDataFormatHeader, BaseConstants.NVP);
                returnMap.Add(BaseConstants.PayPalRequestSourceHeader, SDKName + "-" + SDKVersion);
                returnMap.Add(BaseConstants.PayPalSandboxEmailAddressHeader, GetSandboxEmailAddress());
                returnMap.Add(BaseConstants.PayPalSandboxDeviceIPAddress, GetDeviceIPAddress());
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
		    return returnMap;
	    }

        /// <summary>
        /// Returns Application ID
        /// </summary>
        /// <returns></returns>
	    private string GetApplicationID() 
        {
		    string applicationID = string.Empty;
		    if (Credential is CertificateCredential) 
            {
			    applicationID = ((CertificateCredential) Credential).ApplicationID;
		    } 
            else if (Credential is SignatureCredential) 
            {
			    applicationID = ((SignatureCredential) Credential).ApplicationID;
		    }
		    return applicationID;
	    }

	    private void InitCredential() 
        {
		    if (Credential == null) 
            {
                try
                {
                    Credential = GetCredentials();
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }                
		    }
	    }

        private string GetDeviceIPAddress()
        {
            if (Config.ContainsKey(BaseConstants.ClientIPAddressConfig) && 
                !string.IsNullOrEmpty(Config[BaseConstants.ClientIPAddressConfig]))
            {
                return Config[BaseConstants.ClientIPAddressConfig];
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetSandboxEmailAddress()
        {
            if (Config.ContainsKey(BaseConstants.PayPalSandboxEmailAddressConfig) && 
                !string.IsNullOrEmpty(Config[BaseConstants.PayPalSandboxEmailAddressConfig]))
            {
                return Config[BaseConstants.PayPalSandboxEmailAddressConfig];
            }
            else
            {
                return BaseConstants.PayPalSandboxEmailAddressDefault;
            }
        }    
    }
}
