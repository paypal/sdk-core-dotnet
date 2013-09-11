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
            string clientId = "AQkquBDf1zctJOWGKWUEtKXm6qVhueUEMvXO_-MCI4DQQ4-LWvkDLIN2fGsd";
            string clientSecret = "EL1tVxAjhT7cJimnz5-Nsx9k2reTKSVfErNQF-CmrwJgxRtylkGTKlU4RvrX";
            configurationMap.Add("mode", "sandbox");

            APIContext apiContext = new APIContext();
            apiContext.Config = configurationMap;
            List<string> scopelist = new List<string>();
            scopelist.Add("openid");
            scopelist.Add("email");
            string redirectURI = "https://devtools-paypal.com";
            string redirectURL = Session.GetRedirectURL(clientId, redirectURI, scopelist, apiContext);
            Console.WriteLine(redirectURL);
            CreateFromAuthorizationCodeParameters param = new CreateFromAuthorizationCodeParameters();

            param.setClientId(clientId);
            param.setClientSecret(clientSecret);
            // code you will get back as part of the url after redirection
            param.setCode("VxirnJHENB8k5slnoqZOdmjQcCMJRvbI-ispixwWHke-gsOh6XJaWQNJuTCTp3n3o6ttQs3VoNX1De3HOVpmH2PLN53PPedZcTujzLqYrlTS-CqKHYb5wb0NT2joumArOdEy51D4HgoCa46dxuPMm79nX40RQXRP8J0OQsgrEbhf_Kna");
            Tokeninfo info = Tokeninfo.CreateFromAuthorizationCode(apiContext, param);
            UserinfoParameters userinfoParams = new UserinfoParameters();
            userinfoParams.setAccessToken(info.access_token);
            Userinfo userinfo = Userinfo.GetUserinfo(apiContext, userinfoParams);

            Console.WriteLine("Email" + userinfo.email);

        }
    }
}
