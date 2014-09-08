using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal;

namespace PayPal.UnitTest
{
    /// <summary>
    /// Tests for verifying the data presented in the UserAgent header.
    /// </summary>
    [TestClass]
    public class UserAgentTest
    {
        [TestMethod]
        public void VerifyUserAgentHeader()
        {
            var userAgent = new UserAgentHeader("test-product", "1.5.0");
            var header = userAgent.GetHeader();
            Assert.IsTrue(header.ContainsKey(BaseConstants.UserAgentHeader));
            var userAgentString = userAgent.GetHeader()[BaseConstants.UserAgentHeader];
            Assert.IsTrue(userAgentString.StartsWith("PayPalSDK/test-product 1.5.0"));
        }
    }
}
