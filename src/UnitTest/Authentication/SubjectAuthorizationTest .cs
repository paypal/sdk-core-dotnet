using System;
using NUnit.Framework;
using PayPal.Authentication;

namespace PayPal.UnitTest.Authentication
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