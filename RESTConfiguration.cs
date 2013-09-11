using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal
{
    public class RESTConfiguration
    {
        private string authorizeToken;

        /// <summary>
        /// Authorization Token
        /// </summary>
        public string authorizationToken
        {
            get
            {
                return authorizeToken;
            }
            set
            {
                authorizeToken = value;
            }
        }

        private string requestIdentity;

        /// <summary>
        /// Idempotency Request Id
        /// </summary>
        public string requestId
        {
            private get
            {
                return requestIdentity;
            }
            set
            {
                requestIdentity = value;
            }
        }

        /// <summary>
        /// Dynamic configuration map
        /// </summary>
        private Dictionary<string, string> config;

        /// <summary>
        /// Optional headers map
        /// </summary>
        private Dictionary<string, string> headersMap;

        public RESTConfiguration(Dictionary<string, string> config)
        {
            this.config = ConfigManager.getConfigWithDefaults(config);
        }

        public RESTConfiguration(Dictionary<string, string> config, Dictionary<string, string> headersMap)
        {
            this.config = ConfigManager.getConfigWithDefaults(config);
            this.headersMap = (headersMap == null) ? new Dictionary<string, string>() : headersMap;
        }

        public Dictionary<string, string> GetHeaders()
        {
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                this.headersMap.Add("Authorization", authorizationToken);
            }
            else if (!string.IsNullOrEmpty(GetClientID()) && !string.IsNullOrEmpty(GetClientSecret()))
            {
                this.headersMap.Add("Authorization", "Basic " + EncodeToBase64(GetClientID(), GetClientSecret()));
            }
            this.headersMap.Add("User-Agent", FormUserAgentHeader());
            if (!string.IsNullOrEmpty(requestId))
            {
                this.headersMap.Add("PayPal-Request-Id", requestId);
            }
            return this.headersMap;
        }

        private String GetClientID()
        {
            return this.config.ContainsKey(BaseConstants.CLIENT_ID) ? this.config[BaseConstants.CLIENT_ID] : null;
        }

        private String GetClientSecret()
        {
            return this.config.ContainsKey(BaseConstants.CLIENT_SECRET) ? this.config[BaseConstants.CLIENT_SECRET] : null;
        }

        private String EncodeToBase64(string clientID, string clientSecret)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(clientID + ":" + clientSecret);
                string base64ClientID = Convert.ToBase64String(bytes);
                return base64ClientID;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                throw new PayPalException(ex.Message, ex);
            }
        }

        public static string FormUserAgentHeader()
        {
            string header = null;
            StringBuilder stringBuilder = new StringBuilder("PayPalSDK/"
                    + PayPalResource.SdkID + " " + PayPalResource.SdkVersion
                    + " ");
            string dotNETVersion = GetDotNetVersionHeader();
            stringBuilder.Append(";").Append(dotNETVersion);
            string osVersion = GetOSHeader();
            if (osVersion.Length > 0)
            {
                stringBuilder.Append(";").Append(osVersion);
            }
            header = stringBuilder.ToString();
            return header;
        }

        private static string GetOSHeader()
        {
            string osHeader = string.Empty;
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
            return osHeader;
        }

        private static string GetDotNetVersionHeader()
        {
            string DotNetVersionHeader = "lang=" + "DOTNET;" + "v=" + Environment.Version.ToString().Trim();
            return DotNetVersionHeader;
        }
    }
}
