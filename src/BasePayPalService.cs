using System.Collections.Generic;
using PayPal.Manager;

namespace PayPal
{
    public abstract class BasePayPalService
    {       
        private string Token;
        private string TokenSecret;
        private string LastReq;
        private string LastResp;

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
            this.Token = token;
        }

        public void SetAccessTokenSecret(string tokenSecret)
        {
            this.TokenSecret = tokenSecret;
        }

        public string AccessToken
        {
            get
            {
                return this.Token;
            }
        }

        public string AccessTokenSecret
        {
            get
            {
                return this.TokenSecret;
            }
        }

        public string LastRequest
        {
            get
            {
                return this.LastReq;
            }
        }

        public string LastResponse
        {
            get
            {
                return this.LastResp;
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
            this.LastReq = apiCallHandler.GetPayLoad();
            this.LastResp = apiServ.MakeRequestUsing(apiCallHandler);
            return this.LastResp;
        }
    }
}
