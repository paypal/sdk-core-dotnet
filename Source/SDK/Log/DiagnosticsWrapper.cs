using System;
using System.Diagnostics;
using System.Globalization;

namespace PayPal.Log
{
    /// <summary>
    /// System.Diagnostics wrapper
    /// </summary>
    internal class DiagnosticsWrapper : BaseLogger
    {
        volatile int id = 0;
        TraceSource sourceTrace;

        public DiagnosticsWrapper(Type specifyingType) : base(specifyingType)
        {
            this.sourceTrace = TraceSourceUtil.GetTraceSource(specifyingType);
        }

        public override void Flush()
        {
            if (sourceTrace != null)
            {
                this.sourceTrace.Flush();
            }
        }

        public override void Error(System.Exception exception, string messageFormat, params object[] args)
        {
            sourceTrace.TraceData(TraceEventType.Error, id++, new LogMessage(CultureInfo.InvariantCulture, messageFormat, args), exception);
        }

        public override void Debug(System.Exception exception, string messageFormat, params object[] args)
        {
            sourceTrace.TraceData(TraceEventType.Verbose, id++, new LogMessage(CultureInfo.InvariantCulture, messageFormat, args), exception);
        }

        public override void DebugFormat(string messageFormat, params object[] args)
        {
            sourceTrace.TraceData(TraceEventType.Verbose, id++, new LogMessage(CultureInfo.InvariantCulture, messageFormat, args));
        }

        public override void InfoFormat(string message, params object[] arguments)
        {
            sourceTrace.TraceData(TraceEventType.Information, id++, new LogMessage(CultureInfo.InvariantCulture, message, arguments));
        }

        public override bool IsDebugEnabled 
        { 
            get 
            { 
                return (sourceTrace != null); 
            } 
        }

        public override bool IsErrorEnabled 
        { 
            get 
            { 
                return (sourceTrace != null); 
            } 
        }

        public override bool IsInfoEnabled 
        { 
            get 
            { 
                return (sourceTrace != null); 
            } 
        }
    }
}
