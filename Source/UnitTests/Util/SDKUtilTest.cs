using System;
using System.Collections.Generic;
using PayPal.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
{
    /// <summary>
    ///This is a test class for SDKUtilTest and is intended
    ///to contain all SDKUtilTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SDKUtilTest
    {
        /// <summary>
        /// A test for FormatUriPathForNull
        /// </summary>
        [TestMethod()]
        public void FormatUriPathForNullTest()
        {
            Object[] parameters = null;
            string nullString = SDKUtil.FormatURIPath(null, parameters);
            Assert.IsNull(nullString);
        }

        /// <summary>
        /// A test for FormatURIPathNoPattern
        /// </summary>
        [TestMethod()]
        public void FormatURIPathNoPatternTest()
        {
            Dictionary<string, string> pathParameters = null;
            string pattern = "/a/b/c";
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters);
            Assert.AreEqual(uriPath, pattern);
        }

        /// <summary>
        /// Test for no query string
        /// </summary>
        [TestMethod()]
        public void FormatURIPathNoQS()
        {
            string pattern = "/a/b/{0}";
            Object[] parameters = new Object[] { "replace" };
            string uriPath = SDKUtil.FormatURIPath(pattern, parameters);
            Assert.AreEqual(uriPath, "/a/b/replace");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPath()
        {
            string pattern = "/a/b/{0}?name={1}";
            Object[] parameters = new Object[] { "replace", "nameValue" };
            String uriPath = SDKUtil.FormatURIPath(pattern, parameters);
            Assert.AreEqual(uriPath, "/a/b/replace?name=nameValue");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathWithNull()
        {
            string pattern = "/a/b/{0}?name={1}&age={2}";
            Object[] parameters = new Object[] { "replace", "nameValue", null };
            String uriPath = SDKUtil.FormatURIPath(pattern, parameters);
            Assert.AreEqual(uriPath, "/a/b/replace?name=nameValue");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathWithEmpty()
        {
            string pattern = "/a/b/{0}?name={1}&age=";
            Object[] parameters = new Object[] { "replace", "nameValue", null };
            String uriPath = SDKUtil.FormatURIPath(pattern, parameters);
            Assert.AreEqual(uriPath, "/a/b/replace?name=nameValue");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathTwoQS()
        {
            string pattern = "/a/b/{0}?name={1}&age={2}";
            Object[] parameters = new Object[] { "replace", "nameValue", "1" };
            String uriPath = SDKUtil.FormatURIPath(pattern, parameters);
            Assert.AreEqual(uriPath, "/a/b/replace?name=nameValue&age=1");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMap()
        {
            string pattern = "/a/b/{first}/{second}";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            pathParameters.Add("first", "value1");
            pathParameters.Add("second", "value2");
            String uriPath = SDKUtil.FormatURIPath(pattern, pathParameters);
            Assert.AreEqual(uriPath, "/a/b/value1/value2");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapTraillingSlashTest()
        {
            string pattern = "/a/b/{first}/{second}/";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            pathParameters.Add("first", "value1");
            pathParameters.Add("second", "value2");
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters);
            Assert.AreEqual(uriPath, "/a/b/value1/value2/");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapNullMapTest()
        {
            string pattern = "/a/b/first/second";
            string uriPath = SDKUtil.FormatURIPath(pattern, (Dictionary<string, string>)null);
            Assert.AreEqual(uriPath, "/a/b/first/second");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapIncorrectMapTest()
        {
            string pattern = "/a/b/first/second";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            pathParameters.Add("invalid1", "value1");
            pathParameters.Add("invalid2", "value2");
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters);
            Assert.AreEqual(uriPath, "/a/b/first/second");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(PayPal.Exception.PayPalException))]
        public void FormatURIPathMapInsufficientMapTest()
        {
            string pattern = "/a/b/{first}/{second}";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            pathParameters.Add("first", "value1");
            SDKUtil.FormatURIPath(pattern, pathParameters);
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapNullQueryMapTest()
        {
            string pattern = "/a/b/{first}/{second}";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            pathParameters.Add("first", "value1");
            pathParameters.Add("second", "value2");
            Dictionary<string, string> queryParameters = null;
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters, queryParameters);
            Assert.AreEqual(uriPath, "/a/b/value1/value2");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapEmptyQueryMapTest()
        {
            string pattern = "/a/b/{first}/{second}";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            pathParameters.Add("first", "value1");
            pathParameters.Add("second", "value2");
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters, queryParameters);
            Assert.AreEqual(uriPath, "/a/b/value1/value2");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapQueryMapTest()
        {
            string pattern = "/a/b/first/second";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("query1", "value1");
            queryParameters.Add("query2", "value2");
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters, queryParameters);
            Assert.AreEqual(uriPath, "/a/b/first/second?query1=value1&query2=value2&");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapQueryMapQueryURIPathTest()
        {
            string pattern = "/a/b/first/second?";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("query1", "value1");
            queryParameters.Add("query2", "value2");
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters, queryParameters);
            Assert.AreEqual(uriPath, "/a/b/first/second?query1=value1&query2=value2&");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapQueryMapQueryURIPathEncodeTest()
        {
            string pattern = "/a/b/first/second";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("query1", "value&1");
            queryParameters.Add("query2", "value2");
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters, queryParameters);
            Assert.AreEqual(uriPath, "/a/b/first/second?query1=value%261&query2=value2&");
        }

        /// <summary>
        /// Test FormatUriPath
        /// </summary>
        [TestMethod()]
        public void FormatURIPathMapQueryMapQueryValueURIPathTest()
        {
            string pattern = "/a/b/first/second?alreadypresent=value";
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("query1", "value1");
            queryParameters.Add("query2", "value2");
            string uriPath = SDKUtil.FormatURIPath(pattern, pathParameters, queryParameters);
            Assert.AreEqual(uriPath, "/a/b/first/second?alreadypresent=value&query1=value1&query2=value2&");
        }        
    }
}