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
	    private string RawPayLoad;        
      
        /// <summary>
        /// SDK Configuration
        /// </summary>
        private Dictionary<string, string> Config;
        
        /// <summary>
        /// Gets and sets the Header Element
        /// </summary>
        public string HeaderElement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the Namespaces
        /// </summary>
        public string NamespaceAttributes
        {
            get;
            set;
        }

	    /// <summary>
        /// DefaultSOAPAPICallHandler acts as the base SOAPAPICallHandler.
	    /// </summary>
	    /// <param name="rawPayLoad"></param>
	    /// <param name="namespaces"></param>
	    /// <param name="headerString"></param>
        /// <param name="config"></param>
        public DefaultSOAPAPICallHandler(Dictionary<string, string> config, string rawPayLoad, string attributesNamespace, 
            string headerString) : base()
        {		    
		    this.RawPayLoad = rawPayLoad;
            this.NamespaceAttributes = attributesNamespace;
            this.HeaderElement = headerString;
            this.Config = (config == null) ? ConfigManager.Instance.GetProperties() : config;
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

		    if (RawPayLoad != null) 
            {
			    body = string.Format(SOAPBodyStart, new object[] { null, null, RawPayLoad });
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
