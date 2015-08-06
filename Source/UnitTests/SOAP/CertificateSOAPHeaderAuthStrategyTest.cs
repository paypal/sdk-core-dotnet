using System.Xml;
using PayPal.Authentication;
using PayPal.SOAP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPal.Testing
{
    [TestClass]
    public class CertificateSOAPHeaderAuthStrategyTest
    {
        CertificateCredential certCredential;
        CertificateSOAPHeaderAuthStrategy certSOAPHeaderAuthStrategy;
        SubjectAuthorization subAuthorization;

        [TestMethod]
        public void GenerateHeaderStrategy()
        {
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            certSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            string payload = certSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("testusername", xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);
        }

        [TestMethod]
        public void GenerateHeaderStrategyToken()
        {
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            certSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            TokenAuthorization toknAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            certSOAPHeaderAuthStrategy.ThirdPartyAuthorization = toknAuthorization;
            string payload = certSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            Assert.AreEqual("<ns:RequesterCredentials/>", payload);
        }

        [TestMethod]
        public void GenerateHeaderStrategyThirdPartyAuthorization()
        {
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            certSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            subAuthorization = new SubjectAuthorization("testsubject");
            certSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subAuthorization;
            certCredential.ThirdPartyAuthorization = subAuthorization;
            string payload = certSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("testusername", xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListSubject = xmlDoc.GetElementsByTagName("Subject");
            Assert.IsTrue(xmlNodeListSubject.Count > 0);
            Assert.AreEqual("testsubject", xmlNodeListSubject[0].InnerXml);
        }

        [TestMethod]
        public void ThirdPartyAuthorization()
        {
            certSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            subAuthorization = new SubjectAuthorization("testsubject");
            certSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subAuthorization;
            Assert.IsNotNull(certSOAPHeaderAuthStrategy.ThirdPartyAuthorization);
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