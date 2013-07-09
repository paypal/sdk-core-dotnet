using System;

/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install log4net -OutputDirectory .\packages
    * Add reference from "net20-full" for Visual Studio 2005 or "net35-full" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package log4net
    * Reference is auto-added 
*/
using log4net;

namespace PayPal
{
    public static class LogManagerWrapper
    {
        public static ILog GetLogger(Type type)
        {
            if (LogManager.GetCurrentLoggers().Length == 0)
            {
                LoadConfig();
            }
            return LogManager.GetLogger(type);
        }

        private static void LoadConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}