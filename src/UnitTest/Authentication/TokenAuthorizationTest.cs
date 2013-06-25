using System;
using NUnit.Framework;
using PayPal.Authentication;

namespace PayPal.UnitTest.Authentication
{
    [TestFixture]
    class TokenAuthorizationTest
    {
        [Test, ExpectedException(typeof(ArgumentException))]
	    public void ArgumentExceptionTest() 
        {
            TokenAuthorization toknAuthorization = new TokenAuthorization(null, null);
	    }
    }
}
