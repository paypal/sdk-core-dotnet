using System;
using System.Diagnostics;
using System.Globalization;

namespace PayPal.Log
{
    /// <summary>
    /// System.Diagnostics wrapper
    /// </summary>
    internal class DiagnosticsLogger : BaseLogger
    {
        volatile int id = 0;
        TraceSource sourceTrace;

        public DiagnosticsLogger(Type givenType) : base(givenType)
        {
            this.sourceTrace = TraceSourceUtil.GetTraceSource(givenType);
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
