using System.Text;
using PayPal.Util;

namespace PayPal
{
    public static class BaseConstants
    {
        // Request Method in HTTP Connection
        public const string RequestMethod = "POST";

        // Log file
        public const string PayPalLogFile = "PAYPALLOGFILE";

        //TODO: To be renamed as 'EncodingFormat' as per .NET Naming Conventions
        // Encoding Format
        public static readonly Encoding ENCODING_FORMAT = Encoding.UTF8;
        
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
        public const string PayPalSecurityUserIdHeader = "X-PAYPAL-SECURITY-USERID";

        // PayPal Security Password Header
        public const string PayPalSecurityPasswordHeader = "X-PAYPAL-SECURITY-PASSWORD";

        // PayPal Security Signature Header
        public const string PayPalSecuritySignatureHeader = "X-PAYPAL-SECURITY-SIGNATURE";

        // PayPal Platform Authorization Header
        public const string PayPalAuthorizationPlatformHeader = "X-PAYPAL-AUTHORIZATION";

        // PayPal Merchant Authorization Header
        public const string PayPalAuthorizationMerchantHeader = "X-PP-AUTHORIZATION";

        // PayPal Application Id Header
        public const string PayPalApplicationIdHeader = "X-PAYPAL-APPLICATION-ID";

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
        public const string TestSandboxMode = "security-test-sandbox";

        // Endpoints for various APIs        
        public const string MerchantCertificateLiveEndpoint = "https://api.paypal.com/2.0/";        
        public const string MerchantSignatureLiveEndpoint = "https://api-3t.paypal.com/2.0/";
        public const string PlatformLiveEndpoint = "https://svcs.paypal.com/";
        public const string IPNLiveEndpoint = "https://www.paypal.com/cgi-bin/webscr";
        public const string RESTLiveEndpoint = "https://api.paypal.com/";

        public const string MerchantCertificateSandboxEndpoint = "https://api.sandbox.paypal.com/2.0/";
        public const string MerchantSignatureSandboxEndpoint = "https://api-3t.sandbox.paypal.com/2.0/";
        public const string PlatformSandboxEndpoint = "https://svcs.sandbox.paypal.com/";
        public const string IPNSandboxEndpoint = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        public const string RESTSandboxEndpoint = "https://api.sandbox.paypal.com/";

        public const string MerchantCertificateTestSandboxEndpoint = "https://test-api.sandbox.paypal.com/2.0/";
        public const string MerchantSignatureTestSandboxEndpoint = "https://test-api-3t.sandbox.paypal.com/2.0/";
        public const string PlatformTestSandboxEndpoint = "https://test-svcs.sandbox.paypal.com/";
        public const string IPNTestSandboxEndpoint = "https://test-ipnpb.sandbox.paypal.com/cgi-bin/webscr";
        public const string RESTTestSandboxEndpoint = "https://test-api.sandbox.paypal.com/";

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

        // Configuration key suffix for Credential Application Id
        public const string CredentialApplicationIdConfig = "applicationId";

        // Configuration key suffix for Credential Subject
        public const string CredentialSubjectConfig = "Subject";

        // Configuration key suffix for Credential Signature
        public const string CredentialSignatureConfig = "apiSignature";

        // Configuration key suffix for Credential Certificate Path
        public const string CredentialCertPathConfig = "apiCertificate";

        // Configuration key suffix for Credential Certificate Key
        public const string CredentialCertKeyConfig = "privateKeyPassword";

        // Configuration key suffix for Client Id
        public const string ClientId = "clientId";

        // Configuration key suffix for Client Secret
        public const string ClientSecret = "clientSecret";

        // OpenId Redirect URI config key
        public const string OpenIdRedirectUri = "openid.RedirectUri";

        // OpenId Redirect URI default value
        public const string OpenIdRedirectUriConstant = "https://www.paypal.com/";

        // OAuth endpoint config key
        public const string OAuthEndpoint = "oauth.EndPoint";

        // User Agent HTTP Header
        public const string UserAgentHeader = "User-Agent";

        // Content Type HTTP Header
        public const string ContentTypeHeader = "Content-Type";

        // Application - Json Content Type
        public const string ContentTypeHeaderJson = "application/json";

        // Application - Xml Content Type
        public const string ContentTypeXML = "text/xml";

        // Authorization HTTP Header
        public const string AuthorizationHeader = "Authorization";

        // PayPal Request Id HTTP Header
        public const string PayPalRequestIdHeader = "PayPal-Request-Id";

        // DotNet SdkId for paypal-core
        public const string SdkId = "paypal-core-dotnet";

        // DotNet SdkVersion for paypal-core
        public static string SdkVersion { get { return SDKUtil.GetAssemblyVersionForType(typeof(BaseConstants)); } }
        
        public static class ErrorMessages
        {
            public const string ProfileNull = "APIProfile cannot be null.";
            public const string PayloadNull = "Payload cannot be null or empty.";
            public const string ErrorEndpoint = "Endpoint cannot be empty.";
            public const string ErrorUserName = "API Username cannot be empty";
            public const string ErrorPassword = "API Password cannot be empty.";
            public const string ErrorSignature = "API Signature cannot be empty.";
            public const string ErrorAppId = "Application Id cannot be empty.";
            public const string ErrorCertificate = "Certificate cannot be empty.";
            public const string ErrorPrivateKeyPassword = "Private Key Password cannot be null or empty.";
        }
    }
}
