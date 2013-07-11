using System;
using System.Collections.Generic;
using System.Text;

namespace PayPal
{
    /// <summary>
    /// PayPal User-Agent Header implementation class
    /// </summary>
    class UserAgentHeader
    {
        /// <summary>
        /// Product Id
        /// </summary>
        private string productId;

        /// <summary>
        /// Product Version
        /// </summary>
        private string productVersion;

        /// <summary>
        /// DotNet Version Header
        /// </summary>
        private static string dotnetHeader;

        /// <summary>
        /// Operating System Header
        /// </summary>
        private static string osHeader;

        static UserAgentHeader()
        {
            dotnetHeader = "lang=" + "DOTNET;" + "v=" + Environment.Version.ToString().Trim();
            osHeader = string.Empty;
            if (JCS.OSVersionInfo.OSBits.Equals(JCS.OSVersionInfo.SoftwareArchitecture.Bit64))
            {
                osHeader += "bit=" + 64 + ";";
            }
            else if (JCS.OSVersionInfo.OSBits.Equals(JCS.OSVersionInfo.SoftwareArchitecture.Bit32))
            {
                osHeader += "bit=" + 32 + ";";
            }
            else
            {
                osHeader += "bit=" + "Unknown" + ";";
            }
            osHeader += "os=" + JCS.OSVersionInfo.Name + " " + JCS.OSVersionInfo.Version + ";";
        }

        public UserAgentHeader(string productId, string productVersion)
        {
            this.productId = productId;
            this.productVersion = productVersion;
        }

        public Dictionary<string, string> GetHeader()
        {
            Dictionary<string, string> userAgentDictionary = new Dictionary<string, string>();
            userAgentDictionary.Add(BaseConstants.USER_AGENT_HEADER, FormUserAgentHeader());
            return userAgentDictionary;
        }

        private string FormUserAgentHeader()
        {
            string header = null;
            StringBuilder stringBuilder = new StringBuilder("PayPalSDK/"
                + productId + " " + productVersion + " ");
            stringBuilder.Append(";").Append(dotnetHeader);
            if (!string.IsNullOrEmpty(osHeader))
            {
                stringBuilder.Append(";").Append(osHeader);
            }
            header = stringBuilder.ToString();
            return header;
        }

    }
}
