using System.Collections.Generic;
using PayPal.Manager;

namespace PayPal
{
    public abstract class BasePayPalService
    {       
        private string tokenAccess;
        private string tokenSecretAccess;
        private string requestLast;
        private string responseLast;

        protected Dictionary<string, string> config;

        public BasePayPalService() 
        {
            this.config = ConfigManager.getConfigWithDefaults(ConfigManager.Instance.GetProperties());
        }

        public BasePayPalService(Dictionary<string, string> config) 
        {
            this.config = ConfigManager.getConfigWithDefaults(config);
        }

        public void setAccessToken(string tokenAccess)
        {
            this.tokenAccess = tokenAccess;
        }

        public void setAccessTokenSecret(string tokenSecretAccess)
        {
            this.tokenSecretAccess = tokenSecretAccess;
        }

        public string AccessToken
        {
            get
            {
                return this.tokenAccess;
            }
        }

        public string AccessTokenSecret
        {
            get
            {
                return this.tokenSecretAccess;
            }
        }

        public string LastRequest
        {
            get
            {
                return this.requestLast;
            }
        }

        public string LastResponse
        {
            get
            {
                return this.responseLast;
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
            this.requestLast = apiCallHandler.GetPayLoad();
            this.responseLast = apiServ.MakeRequestUsing(apiCallHandler);
            return this.responseLast;
        }
    }
}
