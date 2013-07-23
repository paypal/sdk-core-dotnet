using System;
using System.Globalization;

namespace PayPal.Log
{
    /// <summary>
    /// Log message
    /// </summary>
    public class LogMessage
    {
        private object[] arguments;
        private IFormatProvider formatProvider;
        private string formatter;

        public object[] Args { get { return this.arguments; } private set { this.arguments = value; } }        
        public IFormatProvider Provider { get { return this.formatProvider; } private set { this.formatProvider = value; } }        
        public string Format { get { return this.formatter; } private set { this.formatter = value; } }

        public LogMessage(string message) : this(CultureInfo.InvariantCulture, message) { }

        public LogMessage(string format, params object[] args) : this(CultureInfo.InvariantCulture, format, args) { }
        
        public LogMessage(IFormatProvider provider, string format, params object[] args)
        {
            this.Args = args;
            this.Format = format;
            this.Provider = provider;
        }

        public override string ToString()
        {
            string formatted = string.Empty;

            if (Args.Length > 0)
            {
                formatted = string.Format(Provider, Format, Args);
            }
            else
            {
                formatted = Format;
            }            

            return formatted;
        }
    }
}
