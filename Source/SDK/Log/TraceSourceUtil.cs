using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace PayPal.Log
{
    /// <summary>
    /// Trace Source utility for the specified Type or the base Type having listener
    /// </summary>
    internal static class TraceSourceUtil
    {
        private static object cacheLock = new object();
        private static Dictionary<string, string> sourceToSourceWithListenersMap = new Dictionary<string, string>();

        /// <summary>
        /// Gets a TraceSource for given Type with SourceLevels.All.
        /// If there are no listeners configured for targetType or one of its "parents", returns null.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static TraceSource GetTraceSource(Type targetType)
        {
            return GetTraceSource(targetType, SourceLevels.All);
        }

        /// <summary>
        /// Gets a TraceSource for given Type and SourceLevels.
        /// If there are no listeners configured for targetType or one of its "parents", returns null.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="sourceLevels"></param>
        /// <returns></returns>
        public static TraceSource GetTraceSource(Type targetType, SourceLevels sourceLevels)
        {
            TraceSource traceSource = GetTraceSourceWithListeners(targetType.FullName, sourceLevels);
            return traceSource;
        }


        // Gets the name of the closest "parent" TraceRoute that has listeners, or null otherwise.
        private static TraceSource GetTraceSourceWithListeners(string name, SourceLevels sourceLevels)
        {
            lock (cacheLock)
            {
                TraceSource traceSource = null;
                string targetName;
                if (!sourceToSourceWithListenersMap.TryGetValue(name, out targetName))
                {
                    traceSource = GetTraceSourceWithListeners_Locked(name, sourceLevels);
                    targetName = traceSource == null ? null : traceSource.Name;
                    sourceToSourceWithListenersMap[name] = targetName;
                }
                else if (targetName != null)
                {
                    traceSource = new TraceSource(targetName, sourceLevels);
                }
                return traceSource;
            }
        }

        // Gets the name of the closest "parent" TraceRoute that has listeners, or null otherwise.
        private static TraceSource GetTraceSourceWithListeners_Locked(string name, SourceLevels sourceLevels)
        {
            string[] parts = name.Split(new char[] { '.' }, StringSplitOptions.None);

            List<string> namesToTest = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (string part in parts)
            {
                if (sb.Length > 0)
                    sb.Append(".");
                sb.Append(part);

                string partialName = sb.ToString();
                namesToTest.Add(partialName);
            }

            namesToTest.Reverse();
            foreach (string testName in namesToTest)
            {
                TraceSource ts = null;
                ts = new TraceSource(testName, sourceLevels);
                // no listeners? skip
                if (ts.Listeners == null || ts.Listeners.Count == 0)
                {
                    ts.Close();
                    continue;
                }
                // more than one listener? use this TraceSource
                if (ts.Listeners.Count > 1)
                    return ts;
                TraceListener listener = ts.Listeners[0];
                // single listener isn't DefaultTraceListener? use this TraceRoute
                if (!(listener is DefaultTraceListener))
                    return ts;
                // single listener is DefaultTraceListener but isn't named Default? use this TraceRoute
                if (!string.Equals(listener.Name, "Default", StringComparison.Ordinal))
                    return ts;

                // not the TraceSource we're looking for, close it
                ts.Close();
            }

            // nothing found? no listeners are configured for any of the names, even the original,
            // so return null to signify failure
            return null;
        }
    }
}
