using System.Collections.Generic;
using PayPal.OpenIdConnect;

#if NUnit
/* NuGet Install
 * Visual Studio 2005
    * Install NUnit -OutputDirectory .\packages
    * Add reference from NUnit.2.6.2
 */
using NUnit.Framework;

namespace PayPal.NUnitTest
{
    [TestFixture]
    class OpenIdTest
    {
        [Ignore]
        public void TestGetAuthUrl()
        {
            Dictionary<string, string> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("ClientId", "dummy");
            configurationMap.Add("ClientSecret",
                    "dummypassword");
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
            TokenInfo info = TokenInfo.CreateFromAuthorizationCode(apiContxt, param);
            UserInfoParameters userInfoParams = new UserInfoParameters();
            userInfoParams.SetAccessToken(info.access_token);
            UserInfo userInformation = UserInfo.GetUserInfo(apiContxt, userInfoParams);
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
{
    [TestClass]
    public class OpenIdTest
    {
        private Tokeninfo info;

        [Ignore]
        public void TestCreateFromAuthorizationCodeDynamic()
        {
            Dictionary<string, string> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("clientId", "");
            configurationMap.Add("clientSecret", "");
            configurationMap.Add("mode", "live");
            APIContext apiContext = new APIContext();
            apiContext.Config = configurationMap;
            CreateFromAuthorizationCodeParameters param = new CreateFromAuthorizationCodeParameters();

            // code you will get back as part of the url after redirection
            param.SetCode("xxxx");
            info = Tokeninfo.CreateFromAuthorizationCode(apiContext, param);
            Assert.AreEqual(true, info.access_token != null);
        }

        [Ignore]
        public void TestCreateFromRefreshTokenDynamic()
        {
            Dictionary<string, string> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("clientId", "");
            configurationMap.Add("clientSecret", "");
            configurationMap.Add("mode", "live");
            APIContext apiContext = new APIContext();
            apiContext.Config = configurationMap;

            CreateFromRefreshTokenParameters param = new CreateFromRefreshTokenParameters();
            info = info.CreateFromRefreshToken(apiContext, param);
            Assert.AreEqual(info.access_token != null, true);
        }

        [Ignore]
        public void TestUserinfoDynamic()
        {
            Dictionary<string, string> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("clientId", "");
            configurationMap.Add("clientSecret", "");
            configurationMap.Add("mode", "live");
            APIContext apiContext = new APIContext();
            apiContext.Config = configurationMap;

            UserinfoParameters userinfoParams = new UserinfoParameters();
            userinfoParams.SetAccessToken(info.access_token);
            Userinfo userinfo = Userinfo.GetUserinfo(apiContext, userinfoParams);
            Assert.AreEqual(userinfo != null, true);
        }

        [TestMethod]
        public void TestGetAuthUrl()
        {
            Dictionary<string, string> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("clientId", "");
            configurationMap.Add("clientSecret", "");
            configurationMap.Add("mode", "live");
            APIContext apiContext = new APIContext();
            apiContext.Config = configurationMap;
            List<string> scopelist = new List<string>();
            scopelist.Add("openid");
            scopelist.Add("email");
            string redirectURI = "http://google.com";
            string redirectURL = Session.GetRedirectUrl(redirectURI, scopelist, apiContext);
            Assert.AreEqual(redirectURL != null, true);
        }
    }
}
#endif
