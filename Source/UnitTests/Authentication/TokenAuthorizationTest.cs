using System;
using PayPal.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
{
    [TestClass]
    public class TokenAuthorizationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TokenAuthorization arguments cannot be empty")]
        public void ArgumentExceptionTest()
        {
            TokenAuthorization toknAuthorization = new TokenAuthorization(null, null);
        }
    }
}