namespace PayPal
{
    public interface SDKVersion
    {

        /// <summary>
        /// SDK ID used in User-Agent HTTP header
        /// </summary>
        /// <returns>SDK ID</returns>
        string GetSDKId();

        /// <summary>
        /// SDK Version used in User-Agent HTTP header
        /// </summary>
        /// <returns>SDK Version</returns>
        string GetSDKVersion();
    }
}
