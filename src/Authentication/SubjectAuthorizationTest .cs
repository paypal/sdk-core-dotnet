using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Authentication;

namespace PayPal
{
    [TestClass]
    public class SubjectAuthorizationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentExceptionTest()
        { 
            try
            {
                SubjectAuthorization subAuthorization = new SubjectAuthorization(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("SubjectAuthorization arguments cannot be null or empty", ex.Message);
                throw;
            }   
        }
    }
}