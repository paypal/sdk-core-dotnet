using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using PayPal.OpenidConnect;
namespace PayPal.UnitTest
{
    [TestFixture]
    class OpenIdTest
    {
        [Ignore]
        public void testGetAuthUrl()
        {
            Dictionary<String, String> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("ClientID", "dummy");
            configurationMap.Add("ClientSecret",
                    "dummypassword");
            configurationMap.Add("mode", "live");
            APIContext apiContext = new APIContext();
            apiContext.Config = configurationMap;
            List<string> scopelist = new List<string>();
            scopelist.Add("openid");
            scopelist.Add("email");
            string redirectURI = "http://google.com";
           string redirectURL = Session.GetRedirectURL(redirectURI,scopelist,apiContext);
           Console.WriteLine(redirectURL);
           CreateFromAuthorizationCodeParameters param = new CreateFromAuthorizationCodeParameters();

            // code you will get back as part of the url after redirection
           param.setCode("wm7qvCMoGwMbtuytIQPhpGn9Gac7nmwVraQIgNp9uQIovP5c-wGn8oB0LmUnhlhse4at4T8XGwXufb7D94YWgIsZpBSzXMwdFkxp4u2oH9dy3HW4");
           Tokeninfo info = Tokeninfo.CreateFromAuthorizationCode(apiContext, param);
           UserinfoParameters userinfoParams = new UserinfoParameters();
           userinfoParams.setAccessToken(info.access_token);
           Userinfo userinfo = Userinfo.GetUserinfo(apiContext, userinfoParams);



        }
    }
}
