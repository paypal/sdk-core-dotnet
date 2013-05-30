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
        private string requestId;

        /// <summary>
        /// Mask request Id
        /// </summary>
        private bool maskRequestIdValue;

        /// <summary>
        /// Dynamic Configuration
        /// </summary>
        private Dictionary<string, string> configValue;

        public APIContext()
        {

        }

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
            this.requestId = requestId;
        }

        public string AccessToken
        {
            get
            {
                return tokenAccess;
            }
        }

        public bool MaskRequestId
        {
            set
            {
                maskRequestIdValue = value;
            }
            get
            {
                return maskRequestIdValue;
            }
        }

        public string RequestID
        {
            get
            {
                string returnId = null;
                if (!MaskRequestId)
                {
                    if (string.IsNullOrEmpty(requestId))
                    {
                        requestId = Convert.ToString(Guid.NewGuid());
                    }
                    returnId = requestId;
                }
                return returnId;
            }
        }

        public Dictionary<string, string> Config
        {
            get
            {
                return configValue;
            }
            set
            {
                this.configValue = value;
            }
        }
    }
}
