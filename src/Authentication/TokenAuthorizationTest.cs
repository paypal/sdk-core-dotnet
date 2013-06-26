using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Authentication;

namespace PayPal
{
    [TestClass]
    public class TokenAuthorizationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
	    public void ArgumentExceptionTest() 
        {
            try
            {
                TokenAuthorization toknAuthorization = new TokenAuthorization(null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("TokenAuthorization arguments cannot be empty", ex.Message);
                throw;
            }
	    }
    }
}
