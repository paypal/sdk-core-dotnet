using System.Collections.Generic;
using PayPal.Authentication;

namespace PayPal
{
    public interface IAPICallPreHandler
    {
        /// <summary>
        /// Returns headers for the HTTP call
        /// with name and value
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetHeaderMap();
        
        /// <summary>
        /// Returns the payload for the API call. 
        /// The implementation should take care 
        /// in formatting the payload appropriately
        /// </summary>
        /// <returns></returns>
	    string GetPayLoad();

	    /// <summary>
        /// Returns the endpoint for the API call
	    /// </summary>
	    /// <returns></returns>
	    string GetEndPoint();

	    /// <summary>
        /// Returns the ICredential configured for the API call
	    /// </summary>
	    /// <returns></returns>
	    ICredential GetCredential();
    }
}
