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

        public BasePayPalService() 
        {
            this.config = ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        public BasePayPalService(Dictionary<string, string> config) 
        {
            this.config = ConfigManager.GetConfigWithDefaults(config);
        }

        public void SetAccessToken(string token)
        {
            this.token = token;
        }

        public void SetAccessTokenSecret(string tokenSecret)
        {
            this.tokenSecret = tokenSecret;
        }

        public string AccessToken
        {
            get
            {
                return this.token;
            }
        }

        public string AccessTokenSecret
        {
            get
            {
                return this.tokenSecret;
            }
        }

        public string LastRequest
        {
            get
            {
                return this.lastReq;
            }
        }

        public string LastResponse
        {
            get
            {
                return this.lastResp;
            }
        }

        /// <summary>
        /// Call method exposed to user
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <returns></returns>
        public string Call(IAPICallPreHandler apiCallHandler)
        {
            APIService apiServ = new APIService(this.config);
            this.lastReq = apiCallHandler.GetPayLoad();
            this.lastResp = apiServ.MakeRequestUsing(apiCallHandler);
            return this.lastResp;
        }
    }
}
