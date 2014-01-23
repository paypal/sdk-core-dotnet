using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using System.Runtime.InteropServices;

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
            dotnetHeader = DotNetVersionHeader;
            osHeader = GetOSHeader();
        }

        /// <summary>
        /// UserAgentHeader Constructor
        /// </summary>
        /// <param name="productId">Product Id, defaults to empty string if null or empty</param>
        /// <param name="productVersion">Product Version, defaults to empty string if null or empty</param>
        public UserAgentHeader(string productId, string productVersion)
        {
            this.productId = String.IsNullOrEmpty(productId) ? "" : productId;
            this.productVersion = String.IsNullOrEmpty(productVersion) ? "" : productVersion;
        }

        /// <summary>
        /// Returns a PayPal specific User-Agent HTTP Header
        /// </summary>
        /// <returns>Dictionary containing User-Agent HTTP Header</returns>
        public Dictionary<string, string> GetHeader()
        {
            Dictionary<string, string> userAgentDictionary = new Dictionary<string, string>();
            userAgentDictionary.Add(BaseConstants.UserAgentHeader, FormUserAgentHeader());
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

        /// <summary>
        /// Returns Operating System Friendly Name. Returns an empty string if the query cannot read from the registry.
        /// </summary>
        private static string OperatingSystemFriendlyName
        {
            get
            {
                //The query SELECT Caption FROM Win32_OperatingSyste gets the OS information from the registry. Azure does not have registry access. Wrapped in try catch.
                try {
                    string result = string.Empty;
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
                    foreach (ManagementObject os in searcher.Get())
                    {
                        result = os["Caption"].ToString();
                    }
                    return result;

                }
                catch (System.Exception) {
                    return string.Empty;
                }
                
            }
        }

#if NET_2_0 || NET_3_5
        private static bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }

        private static bool Is64BitOperatingSystem
        {
            get
            {
                if (Is64BitProcess)
                {
                    return true;
                }
                bool isWow64;
                return ModuleContainsFunction("kernel32.dll", "IsWow64Process") && IsWow64Process(GetCurrentProcess(), out isWow64) && isWow64;
            }
        }

        private static bool ModuleContainsFunction(string moduleName, string methodName)
        {
            IntPtr hModule = GetModuleHandle(moduleName);
            if (hModule != IntPtr.Zero)
            {
                return GetProcAddress(hModule, methodName) != IntPtr.Zero;
            }
            return false;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] out bool isWow64);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        extern static IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        extern static IntPtr GetModuleHandle(string moduleName);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        extern static IntPtr GetProcAddress(IntPtr hModule, string methodName);
#endif
        private static string GetOSHeader()
        {
            string osHeader = string.Empty;

#if NET_2_0 || NET_3_5
            if (Is64BitOperatingSystem)
            {
                osHeader += "bit=" + 64 + ";";
            }
            else 
            {
                osHeader += "bit=" + 32 + ";";
            }
#elif NET_4_0 || NET_4_5 || NET_4_5_1
            if (Environment.Is64BitOperatingSystem)
            {
                osHeader += "bit=" + 64 + ";";
            }
            else
            {
                osHeader += "bit=" + 32 + ";";
            }
#endif
            osHeader += "os=" + OperatingSystemFriendlyName + " " + Environment.OSVersion.Version + ";";
            return osHeader;
        }

        private static string DotNetVersionHeader
        {
            get
            {
                string DotNetVersionHeader = "lang=" + "DOTNET;" + "v=" + Environment.Version.ToString().Trim();
                return DotNetVersionHeader;
            }
        }
    }
}
