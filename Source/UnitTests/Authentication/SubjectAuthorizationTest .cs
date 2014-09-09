using System;
using PayPal.Authentication;
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