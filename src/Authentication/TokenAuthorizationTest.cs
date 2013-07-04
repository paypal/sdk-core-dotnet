using System;
using PayPal.Authentication;

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
    class TokenAuthorizationTest
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ArgumentExceptionTest()
        {
            TokenAuthorization toknAuthorization = new TokenAuthorization(null, null);
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
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
#endif
