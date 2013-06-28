using System;
using System.Collections.Generic;

namespace PayPal
{
    public class APIContext
    {
        /// <summary>
        /// Access Token
        /// </summary>
        private string token;

        /// <summary>
        /// Request ID
        /// </summary>
        private string reqID;

        /// <summary>
        /// Mask Request ID
        /// </summary>
        private bool maskReqID;

        /// <summary>
        /// Dynamic configuration
        /// </summary>
        private Dictionary<string, string> dynamicConfig;

        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public APIContext() { }

        /// <summary>
        /// Access Token required for the call
        /// </summary>
        /// <param name="token"></param>
        public APIContext(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("AccessToken cannot be null");
            }
            this.token = token;
        }

        /// <summary>
        /// Access Token and Request ID required for the call
        /// </summary>
        /// <param name="token"></param>
        /// <param name="requestID"></param>
        public APIContext(string token, string requestID)
            : this(token)
        {
            if (string.IsNullOrEmpty(requestID))
            {
                throw new ArgumentNullException("RequestId cannot be null");
            }
            this.reqID = requestID;
        }

        /// <summary>
        /// Gets the Access Token
        /// </summary>
        public string AccessToken
        {
            get
            {
                return token;
            }
        }

        /// <summary>
        /// Gets and sets the Mask Request ID
        /// </summary>
        public bool MaskRequestID
        {
            get
            {
                return this.maskReqID;
            }
            set
            {
                this.maskReqID = value;
            }
        }
        
        /// <summary>
        /// Gets the Request ID
        /// </summary>
        public string RequestID
        {
            get
            {
                string returnID = null;
                if (!MaskRequestID)
                {
                    if (string.IsNullOrEmpty(reqID))
                    {
                        reqID = Convert.ToString(Guid.NewGuid());
                    }
                    returnID = reqID;
                }
                return returnID;
            }
        }

        /// <summary>
        /// Gets and sets the Dynamic Configuration
        /// </summary>
        public Dictionary<string, string> Config
        {
            get
            {
                return this.dynamicConfig;
            }
            set
            {
                this.dynamicConfig = value;
            }
        }
    }
}
