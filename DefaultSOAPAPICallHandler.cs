using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Manager;
using PayPal.Authentication;

namespace PayPal
{
    public class DefaultSOAPAPICallHandler : IAPICallPreHandler
    {
        /// <summary>
        /// SOAP Envelope Message Formatter String start
        /// </summary>
        private const string SOAPEnvelopeStart = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" {0}>";

	    /// <summary>
        /// SOAP Envelope Message Formatter String end
	    /// </summary>
        private const string SOAPEnvelopeEnd = "</soapenv:Envelope>";

	    /// <summary>
        /// SOAP Header Message Formatter String start
	    /// </summary>
        private const string SOAPHeaderStart = "<soapenv:Header>{1}";

	    /// <summary>
        /// SOAP Header Message Formatter String end
	    /// </summary>
        private const string SOAPHeaderEnd = "</soapenv:Header>";

	    /// <summary>
        /// SOAP Body Message Formatter String start
	    /// </summary>
        private const string SOAPBodyStart = "<soapenv:Body>{2}";
        
        /// <summary>
        /// SOAP Body Message Formatter String end
        /// </summary>
        private const string SOAPBodyEnd = "</soapenv:Body>";

	    /// <summary>
        /// Raw payload from stubs
	    /// </summary>
	    private string rawPayLoad;

	    /// <summary>
        ///  Header element as String
	    /// </summary>
	    private string headElement;
        
        /// <summary>
        /// Namespace attributes as String
        /// </summary>
        private string nmespceAttributes;

        /// <summary>
        /// Endpoint as String
        /// </summary>
        private string endpoint;

        /// <summary>
        /// Gets and sets the Header Element
        /// </summary>
	    public string HeaderElement
        {
            get
            {
                return headElement;
            }
            set
            {
                this.headElement = value;
            }
	    }

        /// <summary>
        /// Gets and sets the Namespaces
        /// </summary>
        public string NamespaceAttributes
        {
            get
            {
                return nmespceAttributes;
            }
            set
            {
                this.nmespceAttributes = value;
            }
        }

	    /// <summary>
        /// DefaultSOAPAPICallHandler acts as the base SOAPAPICallHandler.
	    /// </summary>
	    /// <param name="rawPayLoad"></param>
	    /// <param name="namespaces"></param>
	    /// <param name="headerString"></param>
        public DefaultSOAPAPICallHandler(IConfigManager configMgr, string rawPayLoad, string attributesNamespace, string headerString) : base()
        {		    
		    this.rawPayLoad = rawPayLoad;
            this.nmespceAttributes = attributesNamespace;
		    this.headElement = headerString;
            this.endpoint = configMgr.GetProperty("endpoint");
        }

        //Returns headers for HTTP call
	    public Dictionary<string, string> GetHeaderMap() 
        {
            return new Dictionary<string, string>();
	    }

        /// <summary>
        /// Returns the payload for the API call. 
        /// The implementation should take care
        /// in formatting the payload appropriately
        /// </summary>
        /// <returns></returns>
	    public string GetPayLoad() 
        {
		    StringBuilder payload = new StringBuilder();
		    payload.Append(GetSoapEnvelopeStart());
		    payload.Append(GetSoapHeaderStart());
		    payload.Append(GetSoapHeaderEnd());
		    payload.Append(GetSoapBodyStart());
		    payload.Append(GetSoapBodyEnd());
		    payload.Append(GetSoapEnvelopeEnd());
		    return payload.ToString();
	    }
        
        /// <summary>
        /// Returns the endpoint for the API call
        /// </summary>
        /// <returns></returns>
	    public string GetEndPoint() 
        {
		    return this.endpoint;
	    }

	    public ICredential GetCredential() 
        {
		    return null;
	    }

	    private string GetSoapEnvelopeStart() 
        {
		    string envelope = null;

		    if (nmespceAttributes != null) 
            {
                envelope = string.Format(SOAPEnvelopeStart, new Object[] { nmespceAttributes });
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
		    if (headElement != null) 
            {
			    header = string.Format(SOAPHeaderStart, new Object[] { null,headElement });
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

		    if (rawPayLoad != null) 
            {
			    body = string.Format(SOAPBodyStart, new object[] { null, null, rawPayLoad });
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
    }
}
