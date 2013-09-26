using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Manager;
using PayPal.Authentication;
using System.Xml;
using System.IO;

namespace PayPal
{
    public class DefaultSOAPAPICallHandler : IAPICallPreHandler
    {

        /// <summary>
        /// XML Namespace provider for SOAP serialization
        /// </summary>
        public interface XmlNamespaceProvider
        {
            /// <summary>
            /// Return a Dictionary of XML Namespaces with the entries in the format
		    /// [prefix] = [namespace]
            /// </summary>
            /// <returns>Dictionary of XML Namespaces</returns>
            Dictionary<string, string> GetNamespaceDictionary();
        }

        /// <summary>
        /// XML Namespace identifier
        /// </summary>
        private const string XmlnsAttributePrefix = "xmlns:";

        /// <summary>
        /// SOAP Envelope Qualified Name
        /// </summary>
        private const string SOAPEnvelopeQname = "soapenv:Envelope";

        /// <summary>
        /// SOAP Header Qualified Name
        /// </summary>
        private const string SOAPHeaderQname = "soapenv:Header";

        /// <summary>
        /// SOAP Body Qualified Name
        /// </summary>
        private const string SOAPBodyQname = "soapenv:Body";

        /// <summary>
        /// SOAP Envelope Start Element Tag, used for decorator pattern
        /// </summary>
        private const string SOAPEnvelopeStart = "<" + SOAPEnvelopeQname
                + " " + XmlnsAttributePrefix
                + "soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" {0}>";

        /// <summary>
        /// SOAP Envelope End Element Tag
        /// </summary>
        private const string SOAPEnvelopeEnd = "</" + SOAPEnvelopeQname + ">";

        /// <summary>
        /// SOAP Header Start Element Tag, used for decorator pattern
        /// </summary>
        private const string SOAPHeaderStart = "<" + SOAPHeaderQname
                + ">{1}";

        /// <summary>
        /// SOAP Header End Element Tag
        /// </summary>
        private const string SOAPHeaderEnd = "</" + SOAPHeaderQname + ">";

        /// <summary>
        /// SOAP Body Start Element Tag, used for decorator pattern
        /// </summary>
        private const string SOAPBodyStart = "<" + SOAPBodyQname
                + ">{2}";

        /// <summary>
        /// SOAP Body End Element Tag
        /// </summary>
        private const string SOAPBodyEnd = "</" + SOAPBodyQname + ">";

        /// <summary>
        /// SOAP Envelope Namespace
        /// </summary>
        private const string SOAPEnvelopeNS = "http://schemas.xmlsoap.org/soap/envelope/";

        /// <summary>
        /// Raw payload from stubs
	    /// </summary>
	    private string rawPayload;        
      
        /// <summary>
        /// SDK Configuration
        /// </summary>
        private Dictionary<string, string> Config;

        /// <summary>
        /// Header Element
        /// </summary>
        private string elementHeader;

        /// <summary>
        /// Namespaces
        /// </summary>
        public string attributeNamespaces;

        /// <summary>
        /// APIContext instance
        /// </summary>
        public APIContext ApiContext;

        /// <summary>
        /// API Method Name
        /// </summary>
        public string MethodName;

        /// <summary>
        /// Instance of XMLMessageSerializer for SOAP Header
        /// </summary>
        public XMLMessageSerializer HeaderContent;

        /// <summary>
        /// Instance of XMLMessageSerializer for SOAP Body
        /// </summary>
        public XMLMessageSerializer BodyContent;

        /// <summary>
        /// XML Namespace provider for SOAP serialization
        /// </summary>
        private static XmlNamespaceProvider XmlNamespaceProviderValue;

        /// <summary>
        /// XML Namespace provider for SOAP serialization
        /// </summary>
        public static XmlNamespaceProvider XMLNamespaceProvider
        {
            get
            {
                return XmlNamespaceProviderValue;
            }
            set
            {
                XmlNamespaceProviderValue = value;
            }
        }

        /// <summary>
        /// Gets and sets the Header Element
        /// </summary>
        public string HeaderElement
        {
            get
            {
                return this.elementHeader;
            }
            set
            {
                this.elementHeader = value;
            }
        }
        
        /// <summary>
        /// Gets and sets the Namespaces
        /// </summary>
        public string NamespaceAttributes
        {
            get
            {
                return this.attributeNamespaces;

            }
            set
            {
                this.attributeNamespaces = value;
            }
        }

        /// <summary>
        /// DefaultSOAPAPICallHandler acts as the base SOAPAPICallHandler.
	    /// </summary>
	    /// <param name="rawPayload"></param>
	    /// <param name="namespaces"></param>
	    /// <param name="headerString"></param>
        /// <param name="config"></param>
        public DefaultSOAPAPICallHandler(Dictionary<string, string> config, string rawPayload, string attributesNamespace, 
            string headerString) : base()
        {		    
		    this.rawPayload = rawPayload;
            this.NamespaceAttributes = attributesNamespace;
            this.HeaderElement = headerString;
            this.Config = (config == null) ? ConfigManager.Instance.GetProperties() : config;
	    }

        public DefaultSOAPAPICallHandler(XMLMessageSerializer soapBodyContent,
            APIContext apiContext, Dictionary<String, String> Config,
            string methodName)
        {
            this.ApiContext = apiContext;
            this.MethodName = methodName;
            this.Config = ConfigManager.GetConfigWithDefaults(apiContext.Config == null ? Config : apiContext.Config);
            HeaderContent = apiContext.SOAPHeader;
            this.BodyContent = soapBodyContent;
        }

        //Returns headers for HTTP call
	    public Dictionary<string, string> GetHeaderMap() 
        {
            Dictionary<string, string> headerDictionary = null;
            if (ApiContext != null)
            {
                headerDictionary = ApiContext.HTTPHeaders;
            }
            if (headerDictionary == null)
            {
                headerDictionary = new Dictionary<string, string>();
            }
            headerDictionary.Add(BaseConstants.ContentTypeHeader, BaseConstants.ContentTypeXML);
            return headerDictionary;
	    }

        /// <summary>
        /// Returns the payload for the API call. 
        /// The implementation should take care
        /// in formatting the payload appropriately
        /// </summary>
        /// <returns></returns>
	    public string GetPayload() 
        {
            string payload = null;
            if (BodyContent != null)
            {
                try
                {
                    payload = GetSOAPEnvelope();
                }
                catch { }
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(GetSoapEnvelopeStart());
                stringBuilder.Append(GetSoapHeaderStart());
                stringBuilder.Append(GetSoapHeaderEnd());
                stringBuilder.Append(GetSoapBodyStart());
                stringBuilder.Append(GetSoapBodyEnd());
                stringBuilder.Append(GetSoapEnvelopeEnd());
                payload = stringBuilder.ToString();
            }
            return payload;
	    }
        
        /// <summary>
        /// Returns the endpoint for the API call
        /// </summary>
        /// <returns></returns>
	    public string GetEndpoint() 
        {
		    return Config[BaseConstants.EndpointConfig];
	    }

	    public ICredential GetCredential() 
        {
		    return null;
	    }

	    private string GetSoapEnvelopeStart() 
        {
		    string envelope = null;

            if (NamespaceAttributes != null) 
            {
                envelope = string.Format(SOAPEnvelopeStart, new Object[] { NamespaceAttributes });
		    } 
            else 
            {
			    envelope = SOAPEnvelopeStart;
		    }
		    return envelope;
	    }

	    private string GetSoapEnvelopeEnd()
        {
            return SOAPEnvelopeEnd;
        }

	    private string GetSoapHeaderStart() 
        {
		    string header = null;
            if (HeaderElement != null) 
            {
                header = string.Format(SOAPHeaderStart, new Object[] { null, HeaderElement });
		    } 
            else 
            {
			    header = SOAPHeaderStart;
		    }
		    return header;
	    }

	    private string GetSoapHeaderEnd() 
        {
		    return SOAPHeaderEnd;
	    }

	    private string GetSoapBodyStart() 
        {
		    string body = null;

		    if (rawPayload != null) 
            {
			    body = string.Format(SOAPBodyStart, new object[] { null, null, rawPayload });
		    } 
            else 
            {
			    body = SOAPBodyStart;
		    }
		    return body;
	    }

	    private string GetSoapBodyEnd()
        {
            return SOAPBodyEnd;
	    }

        /// <summary>
        /// Returns the SOAP Envelope as a String
        /// </summary>
        /// <returns>SOAP Envelope as a String</returns>
        private string GetSOAPEnvelope()
        {
            return NodeToString(GetSOAPEnvelopeAsNode());
        }

        /// <summary>
        /// Returns the SOAP Envelope as a XmlNode
        /// </summary>
        /// <returns>SOAP Envelope as a XmlNode</returns>
        private XmlNode GetSOAPEnvelopeAsNode()
        {
            XmlDocument xmlDocument = GetSOAPEnvelopeAsDocument();
            if (BodyContent != null)
            {
                xmlDocument.DocumentElement.GetElementsByTagName(SOAPBodyQname.Substring(SOAPBodyQname.IndexOf(':') + 1), SOAPEnvelopeNS).Item(0).AppendChild(xmlDocument.ImportNode(GetNode(BodyContent.ToXMLString()), true));
            }
            if (HeaderContent != null)
            {
                xmlDocument.DocumentElement.GetElementsByTagName(SOAPHeaderQname.Substring(SOAPHeaderQname.IndexOf(':') + 1), SOAPEnvelopeNS).Item(0).AppendChild(xmlDocument.ImportNode(GetNode(HeaderContent.ToXMLString()), true));
            }
            return xmlDocument.DocumentElement;
        }

        /// <summary>
        ///  Creates a XmlDocument for SOAP Envelope
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetSOAPEnvelopeAsDocument()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement soapEnvelope = xmlDocument.CreateElement(SOAPEnvelopeQname, SOAPEnvelopeNS);
            xmlDocument.AppendChild(soapEnvelope);
            SetNamespaces(xmlDocument.DocumentElement);
            XmlElement soapHeader = xmlDocument.CreateElement(SOAPHeaderQname, SOAPEnvelopeNS);
            XmlElement soapBody = xmlDocument.CreateElement(SOAPBodyQname, SOAPEnvelopeNS);
            soapEnvelope.AppendChild(soapHeader);
            soapEnvelope.AppendChild(soapBody);
            return xmlDocument;
        }

        /// <summary>
        ///  Adds namespaces to the provided XmlElement
        /// </summary>
        /// <param name="xmlElement">XmlElement instance</param>
        private void SetNamespaces(XmlElement xmlElement)
        {
            if (xmlElement != null && XMLNamespaceProvider != null)
            {
                foreach (KeyValuePair<string, string> entry in XMLNamespaceProvider.GetNamespaceDictionary())
                {
                    xmlElement.SetAttribute(XmlnsAttributePrefix + entry.Key.Trim(), entry.Value.Trim());
                }
            }
        }

        /// <summary>
        /// Returns a XmlNode object by parsing the string content of the node
        /// </summary>
        /// <param name="nodeAsString">string Content of the XmlNode</param>
        /// <returns></returns>
        private XmlNode GetNode(string nodeAsString)
        {
            XmlNode xmlNode = null;
            if (!String.IsNullOrEmpty(nodeAsString))
            {
                XmlDocument doc = new XmlDocument();
                NameTable nameTable = new NameTable();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(nameTable);
                foreach (KeyValuePair<string, string> entry in XMLNamespaceProvider.GetNamespaceDictionary())
                {
                    nsmgr.AddNamespace(entry.Key, entry.Value);
                }
                XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
                XmlReaderSettings xset = new XmlReaderSettings();
                xset.ConformanceLevel = ConformanceLevel.Fragment;
                XmlReader rd = XmlReader.Create(new StringReader(nodeAsString), xset, context);
                doc.Load(rd);
                xmlNode = doc.DocumentElement;
            }
            return xmlNode;
        }

        /// <summary>
        /// Returns the content of the XmlNode object as a string
        /// </summary>
        /// <param name="xmlNode">XmlNode instance</param>
        /// <returns></returns>
        private string NodeToString(XmlNode xmlNode)
        {
            string nodeString = null;
            if (xmlNode != null)
            {
                nodeString = xmlNode.OuterXml;
            }
            return nodeString;
        }
    }
}
