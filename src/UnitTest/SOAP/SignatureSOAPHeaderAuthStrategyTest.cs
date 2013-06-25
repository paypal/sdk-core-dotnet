using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using PayPal.Authentication;
using PayPal.SOAP;


namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class SignatureSOAPHeaderAuthStrategyTest
    {
        SignatureCredential signCredential;
        SignatureSOAPHeaderAuthStrategy signSOAPHeaderAuthStrategy;
        SubjectAuthorization subAuthorization;

        [Test]
        public void GenerateHeaderStrategy()
        {
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            signSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            string payload = signSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("testusername", xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListSignature = xmlDoc.GetElementsByTagName("Signature");
            Assert.IsTrue(xmlNodeListSignature.Count > 0);
            Assert.AreEqual("testsignature", xmlNodeListSignature[0].InnerXml);
        }

        [Test]
        public void GenerateHeaderStrategyToken()
        {
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            signSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            TokenAuthorization toknAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            signSOAPHeaderAuthStrategy.ThirdPartyAuthorization = toknAuthorization;
            signCredential.ThirdPartyAuthorization = toknAuthorization;
            string payload = signSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            Assert.AreEqual("<ns:RequesterCredentials/>", payload);
        }               

        [Test]
        public void GenerateHeaderStrategyThirdParty()
        {
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            signSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            subAuthorization = new SubjectAuthorization("testsubject");
            signSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subAuthorization;
            signCredential.ThirdPartyAuthorization = subAuthorization;
            string payload = signSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);            
            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList NodeListUsername = xmlDoc.GetElementsByTagName("Username");            
            Assert.IsTrue(NodeListUsername.Count > 0);
            Assert.AreEqual("testusername", NodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListSignature = xmlDoc.GetElementsByTagName("Signature");
            Assert.IsTrue(xmlNodeListSignature.Count > 0);
            Assert.AreEqual("testsignature", xmlNodeListSignature[0].InnerXml);
            XmlNodeList xmlNodeListSubject = xmlDoc.GetElementsByTagName("Subject");
            Assert.IsTrue(xmlNodeListSubject.Count > 0);
            Assert.AreEqual("testsubject", xmlNodeListSubject[0].InnerXml);
        }       

        [Test]
        public void ThirdPartyAuthorization()
        {
            signSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            subAuthorization = new SubjectAuthorization("testsubject");
            signSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subAuthorization;
            Assert.IsNotNull(signSOAPHeaderAuthStrategy.ThirdPartyAuthorization);
            Assert.AreEqual("testsubject", ((SubjectAuthorization)(signSOAPHeaderAuthStrategy.ThirdPartyAuthorization)).Subject);
        }
        
        private XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlString = xmlString.Replace("ns:", string.Empty);
            xmlString = xmlString.Replace("ebl:", string.Empty);
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }
    }
}
