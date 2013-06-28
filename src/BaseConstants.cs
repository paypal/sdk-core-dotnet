using System.Text;

namespace PayPal
{
    public static class BaseConstants
    {
        // Request Method in HTTP Connection
        public const string RequestMethod = "POST";

        // Log file
        public const string PayPalLogFile = "PAYPALLOGFILE";

        // Encoding Format
        public static readonly Encoding EncodingFormat = Encoding.UTF8;
        
        // Account Prefix
        public const string AccountPrefix = "acct";

        // Sandbox Default Email Address
        public const string PayPalSandboxEmailAddressDefault = "pp.devtools@gmail.com";
        
        // SOAP Format
        public const string SOAP = "SOAP";
        
        // NVP Format
        public const string NVP = "NV";
        
        // HTTP Header Constants
        // PayPal Security UserId Header
        public const string PayPalSecurityUserIDHeader = "X-PAYPAL-SECURITY-USERID";

        // PayPal Security Password Header
        public const string PayPalSecurityPasswordHeader = "X-PAYPAL-SECURITY-PASSWORD";

        // PayPal Security Signature Header
        public const string PayPalSecuritySignatureHeader = "X-PAYPAL-SECURITY-SIGNATURE";

        // PayPal Platform Authorization Header
        public const string PayPalAuthorizationPlatformHeader = "X-PAYPAL-AUTHORIZATION";

        // PayPal Merchant Authorization Header
        public const string PayPalAuthorizationMerchantHeader = "X-PP-AUTHORIZATION";

        // PayPal Application ID Header
        public const string PayPalApplicationIDHeader = "X-PAYPAL-APPLICATION-ID";

        // PayPal Request Data Header
        public const string PayPalRequestDataFormatHeader = "X-PAYPAL-REQUEST-DATA-FORMAT";

        // PayPal Request Data Header
        public const string PayPalResponseDataFormatHeader = "X-PAYPAL-RESPONSE-DATA-FORMAT";

        // PayPal Request Source Header
        public const string PayPalRequestSourceHeader = "X-PAYPAL-REQUEST-SOURCE";
        
        // PayPal Sandbox Email Address Header
        public const string PayPalSandboxDeviceIPAddress = "X-PAYPAL-DEVICE-IPADDRESS";

        // PayPal Sandbox Email Address Header
        public const string PayPalSandboxEmailAddressHeader = "X-PAYPAL-SANDBOX-EMAIL-ADDRESS";

        // Allowed application modes
        public const string LiveMode = "live";
        public const string SandboxMode = "sandbox";

        // Endpoints for various APIs        
        public const string MerchantCertificateLiveEndpoint = "https://api.paypal.com/2.0/";        
        public const string MerchantSignatureLiveEndpoint = "https://api-3t.paypal.com/2.0/";
        public const string PlatformLiveEndpoint = "https://svcs.paypal.com/";
        public const string IPNLiveEndpoint = "https://ipnpb.paypal.com/cgi-bin/webscr";
        public const string RESTSandboxEndpoint = "https://api.sandbox.paypal.com/";

        public const string MerchantCertificateSandboxEndpoint = "https://api.sandbox.paypal.com/2.0/";
        public const string MerchantSignatureSandboxEndpoint = "https://api-3t.sandbox.paypal.com/2.0/";
        public const string PlatformSandboxEndpoint = "https://svcs.sandbox.paypal.com/";
        public const string IPNSandboxEndpoint = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        public const string RESTLiveEndpoint = "https://api.paypal.com/";

        // Configuration key for application mode
        public const string ApplicationModeConfig = "mode";

        // Configuration key for End point
        public const string EndpointConfig = "endpoint";

        // Configuration key for IPN endpoint 
        public const string IPNEndpointConfig = "IPNEndpoint";

        // Configuration key for IPAddress
        public const string ClientIPAddressConfig = "IPAddress";
       
        // Configuration key for Email Address
        public const string PayPalSandboxEmailAddressConfig = "sandboxEmailAddress";

        // Configuration key for HTTP Proxy Address
        public const string HttpProxyAddressConfig = "proxyAddress";

        // Configuration key for HTTP Proxy Credential
        public const string HttpProxyCredentialConfig = "proxyCredentials";

        // Configuration key for HTTP Connection Timeout
        public const string HttpConnectionTimeoutConfig = "connectionTimeout";

        // Configuration key for HTTP Connection Retry
        public const string HttpConnectionRetryConfig = "requestRetries";

        // Configuration key suffix for Credential Username
        public const string CredentialUserNameConfig = "apiUsername";

        // Configuration key suffix for Credential Password
        public const string CredentialPasswordConfig = "apiPassword";

        // Configuration key suffix for Credential Application ID
        public const string CredentialApplicationIDConfig = "applicationId";

        // Configuration key suffix for Credential Subject
        public const string CredentialSubjectConfig = "Subject";

        // Configuration key suffix for Credential Signature
        public const string CredentialSignatureConfig = "apiSignature";

        // Configuration key suffix for Credential Certificate Path
        public const string CredentialCertPathConfig = "apiCertificate";

        // Configuration key suffix for Credential Certificate Key
        public const string CredentialCertKeyConfig = "privateKeyPassword";

        // Configuration key suffix for Client Id
        public const string ClientID = "clientId";

        // Configuration key suffix for Client Secret
        public const string ClientSecret = "clientSecret";

        public const string OpenIDRedirectURI = "openid.RedirectUri";

        public const string OpenIDRedirectURIConstant = "https://www.paypal.com/webapps/auth/protocol/openidconnect";

        public const string OAuthEndpoint = "oauth.EndPoint";
        
        public static class ErrorMessages
        {
            public const string ProfileNull = "APIProfile cannot be null.";
            public const string PayloadNull = "PayLoad cannot be null or empty.";
            public const string ErrorEndpoint = "Endpoint cannot be empty.";
            public const string ErrorUserName = "API Username cannot be empty";
            public const string ErrorPassword = "API Password cannot be empty.";
            public const string ErrorSignature = "API Signature cannot be empty.";
            public const string ErrorAppID = "Application ID cannot be empty.";
            public const string ErrorCertificate = "Certificate cannot be empty.";
            public const string ErrorPrivateKeyPassword = "Private Key Password cannot be null or empty.";
        }
    }
}
