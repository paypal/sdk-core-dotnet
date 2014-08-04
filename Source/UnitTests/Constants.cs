using System.IO;

namespace PayPal
{
    static class Constants
    {
	    public const string APIUserName = "jb-us-seller_api1.paypal.com";
        public const string APIPassword = "WX4WTU3S8MY44S7F";
        public const string APISignature = "AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy";
        public const string CertificateAPIUserName = "certuser_biz_api1.paypal.com";	    
        public const string CertificateAPIPassword = "D6JNKKULHN3G5B8A";
        public static readonly string CertificatePath = Path.GetFullPath("sdk-cert.p12");
        public const string CertificatePassword = "password";
	    public const string ApplicationId = "APP-80W284485P519543T";
	    public const string APIEndpointNVP = "https://svcs.sandbox.paypal.com/";
        public const string APIEndpointSOAP = "https://api-3t.sandbox.paypal.com/2.0";
	    public const string AccessToken = "AhARVvVPBOlOICu1xkH29I53ZvD.0p-vYauZhyWnKxMb5861PXG82Q";
	    public const string TokenSecret = "Ctsch..an4Bgx0I75X8CTNxqRn8";
        public const string PayloadNVP = @"requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
        public const string PayloadSOAP = @"<ns:GetBalanceReq><ns:GetBalanceRequest><ebl:Version>94.0</ebl:Version></ns:GetBalanceRequest></ns:GetBalanceReq>";
    }
}
