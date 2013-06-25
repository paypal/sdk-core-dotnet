using System;
using System.Collections.Generic;

namespace PayPal
{
    public class APIContext
    {
        /// <summary>
        /// Access Token
        /// </summary>
        private string tokenAccess;

        /// <summary>
        /// Request ID
        /// </summary>
        private string requestID;

        /// <summary>
        /// Access Token required for the call
        /// </summary>
        /// <param name="tokenAccess"></param>
        public APIContext(string tokenAccess)
        {
            if (string.IsNullOrEmpty(tokenAccess))
            {
                throw new ArgumentNullException("AccessToken cannot be null");
            }
            this.tokenAccess = tokenAccess;
        }

        /// <summary>
        /// Access Token and Request ID required for the call
        /// </summary>
        /// <param name="tokenAccess"></param>
        /// <param name="requestId"></param>
        public APIContext(string tokenAccess, string requestId)
            : this(tokenAccess)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentNullException("RequestId cannot be null");
            }
            this.requestID = requestId;
        }

        public string AccessToken
        {
            get
            {
                return tokenAccess;
            }
        }

        /// <summary>
        /// Mask Request ID
        /// </summary>
        public bool MaskRequestID
        {   
            get;
            set;
        }
         
        /// <summary>
        /// Request ID
        /// </summary>
        public string RequestID
        {
            get
            {
                string returnId = null;
                if (!MaskRequestID)
                {
                    if (string.IsNullOrEmpty(requestID))
                    {
                        requestID = Convert.ToString(Guid.NewGuid());
                    }
                    returnId = requestID;
                }
                return returnId;
            }
        }

        /// <summary>
        /// Dynamic Configuration
        /// </summary>
        public Dictionary<string, string> Config
        {
            get;
            set;
        }
    }
}
