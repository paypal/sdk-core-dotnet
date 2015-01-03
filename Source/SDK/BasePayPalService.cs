using System.Collections.Generic;
using PayPal.Manager;

namespace PayPal
{
    public abstract class BasePayPalService
    {       
        private string token;
        private string tokenSecret;
        private string lastReq;
        private string lastResp;

        protected Dictionary<string, string> config;

        protected BasePayPalService() 
        {
            this.config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        protected BasePayPalService(Dictionary<string, string> config) 
        {
            this.config = ConfigManager.GetConfigWithDefaults(config);
        }

        //public string AccessToken
        //{
        //    get
        //    {
        //        return this.token;
        //    }
        //    set
        //    {
        //        this.token = value;
        //    }
        //}

        //[Obsolete("'getAccessToken()' is obsolete: 'The recommended alternative is AccessToken.'")]
        public string getAccessToken()
        {
            return this.token;
        }

        //[Obsolete("'SetAccessToken(string token)' is obsolete: 'The recommended alternative is AccessToken.'")]
        public void SetAccessToken(string token)
        {
            this.token = token;
        }

        //public string AccessTokenSecret
        //{
        //    get
        //    {
        //        return this.tokenSecret;
        //    }
        //    set
        //    {
        //        this.tokenSecret = value;
        //    }
        //}

        //[Obsolete("'getAccessTokenSecret()' is obsolete: 'The recommended alternative is AccessTokenSecret.'")]
        public string getAccessTokenSecret()
        {
            return this.tokenSecret;            
        }

        //[Obsolete("'SetAccessTokenSecret(string tokenSecret)' is obsolete: 'The recommended alternative is AccessTokenSecret.'")]
        public void SetAccessTokenSecret(string tokenSecret)
        {
            this.tokenSecret = tokenSecret;
        }

        //public string LastRequest
        //{
        //    get
        //    {
        //        return this.lastReq;
        //    }
        //}

        //[Obsolete("'getLastRequest()' is obsolete: 'The recommended alternative is LastRequest.'")]
        public string getLastRequest()
        {
            return this.lastReq;
        }

        //public string LastResponse
        //{
        //    get
        //    {
        //        return this.lastResp;
        //    }
        //}

        //[Obsolete("'getLastResponse()' is obsolete: 'The recommended alternative is LastResponse.'")]
        public string getLastResponse()
        {
            return this.lastResp;
        }

        /// <summary>
        /// Call method exposed to user
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <returns></returns>
        public string Call(IAPICallPreHandler apiCallHandler)
        {
            APIService apiServ = new APIService(this.config);
            this.lastReq = apiCallHandler.GetPayload();
            this.lastResp = apiServ.MakeRequestUsing(apiCallHandler);
            return this.lastResp;
        }
    }
}
