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
        /// Request Id
        /// </summary>
        private string reqId;

        /// <summary>
        /// Mask Request Id
        /// </summary>
        private bool maskReqId;

        /// <summary>
        /// Dynamic configuration
        /// </summary>
        private Dictionary<string, string> dynamicConfig;

        /// <summary>
        /// HTTP Headers
        /// </summary>
        private Dictionary<string, string> httpHeaders;

        /// <summary>
        /// 
        /// </summary>
        private object applicationHeaderValue;

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
        /// Access Token and Request Id required for the call
        /// </summary>
        /// <param name="token"></param>
        /// <param name="requestId"></param>
        public APIContext(string token, string requestId)
            : this(token)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentNullException("RequestId cannot be null");
            }
            this.reqId = requestId;
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
        /// Gets and sets the Mask Request Id
        /// </summary>
        public bool MaskRequestId
        {
            get
            {
                return this.maskReqId;
            }
            set
            {
                this.maskReqId = value;
            }
        }
        
        /// <summary>
        /// Gets the Request Id
        /// </summary>
        public string RequestId
        {
            get
            {
                string returnId = null;
                if (!MaskRequestId)
                {
                    if (string.IsNullOrEmpty(reqId))
                    {
                        reqId = Convert.ToString(Guid.NewGuid());
                    }
                    returnId = reqId;
                }
                return returnId;
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

        /// <summary>
        /// Gets and sets HTTP Headers
        /// </summary>
        public Dictionary<string, string> HeadersMap
        {
            get
            {
                return this.httpHeaders;
            }
            set
            {
                this.httpHeaders = value;
            }
        }

        public object ApplicationHeader
        {
            get
            {
                return this.applicationHeaderValue;
            }
            set
            {
                this.applicationHeaderValue = value;
            }
        }
    }
}
