using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.OpenIDConnect;

namespace PayPal
{
    [TestClass]
    public class OpenIDTest
    {
        [Ignore]
        public void GetAuthUrlTest()
        {
            Dictionary<string, string> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("ClientID", "dummy");
            configurationMap.Add("ClientSecret", "dummypassword");
            configurationMap.Add("mode", "live");
            APIContext apiContxt = new APIContext();
            apiContxt.Config = configurationMap;
            List<string> scopeList = new List<string>();
            scopeList.Add("openid");
            scopeList.Add("email");
            string redirectURI = "http://google.com";
            string redirectURL = Session.GetRedirectURL(redirectURI, scopeList, apiContxt);
            CreateFromAuthorizationCodeParameters param = new CreateFromAuthorizationCodeParameters();

            // code you will get back as part of the url after redirection
            param.SetCode("wm7qvCMoGwMbtuytIQPhpGn9Gac7nmwVraQIgNp9uQIovP5c-wGn8oB0LmUnhlhse4at4T8XGwXufb7D94YWgIsZpBSzXMwdFkxp4u2oH9dy3HW4");
            Tokeninfo infoToken = Tokeninfo.CreateFromAuthorizationCode(apiContxt, param);
            UserinfoParameters userinfoParams = new UserinfoParameters();
            userinfoParams.setAccessToken(infoToken.access_token);
            Userinfo userinfo = Userinfo.GetUserinfo(apiContxt, userinfoParams);
        }
    }
}
