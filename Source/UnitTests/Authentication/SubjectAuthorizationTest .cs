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
    class SubjectAuthorizationTest
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ArgumentExceptionTest()
        {
            SubjectAuthorization subAuthorization = new SubjectAuthorization(null);
        }
    }
}
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.UnitTest
{
    [TestClass]
    public class SubjectAuthorizationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "SubjectAuthorization arguments cannot be null or empty")]
        public void ArgumentExceptionTest()
        {
            SubjectAuthorization subAuthorization = new SubjectAuthorization(null);   
        }
    }
}
#endif