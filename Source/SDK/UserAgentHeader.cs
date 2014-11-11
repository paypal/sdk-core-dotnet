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
    public class UserAgentHeader
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
        /// UserAgentHeader Constructor
        /// </summary>
        /// <param name="productId">Product Id, defaults to empty string if null or empty</param>
        /// <param name="productVersion">Product Version, defaults to empty string if null or empty</param>
        public UserAgentHeader(string productId, string productVersion)
        {
            this.productId = productId;
            this.productVersion = productVersion;
        }

        /// <summary>
        /// Returns a PayPal specific User-Agent HTTP Header
        /// </summary>
        /// <returns>Dictionary containing User-Agent HTTP Header</returns>
        public Dictionary<string, string> GetHeader()
        {
            Dictionary<string, string> userAgentDictionary = new Dictionary<string, string>();
            userAgentDictionary.Add(BaseConstants.UserAgentHeader, this.GetUserAgentHeader());
            return userAgentDictionary;
        }

        /// <summary>
        /// Creates the signature for the UserAgent header.
        /// </summary>
        /// <returns>A string containing the signature for the UserAgent header.</returns>
        private string GetUserAgentHeader()
        {
            StringBuilder header = new StringBuilder("PayPalSDK/");
            header.Append(this.productId);
            header.Append(" " + this.productVersion);
            header.Append(" (");

            header.Append(string.Join(";", new string[] 
            {
                FormatUserAgentParameter("core", BaseConstants.SdkVersion),
                FormatUserAgentParameter("lang", "DOTNET"),
                FormatUserAgentParameter("v", DotNetVersion),
                FormatUserAgentParameter("clr", DotNetClrVersion),
                FormatUserAgentParameter("bit", OperatingSystemBitness),
                FormatUserAgentParameter("os", OperatingSystemName)
            }));
            header.Append(")");
            return header.ToString();
        }

        /// <summary>
        /// Formats a parameter name and value to be used in the signature of a UserAgent header.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A formatted string containing both the parameter name and value.</returns>
        private string FormatUserAgentParameter(string name, object value)
        {
            return string.Format("{0}={1}", name, value);
        }

#if NET_2_0 || NET_3_5
        /// <summary>
        /// Returns whether or not the current process is running as a 64-bit process.
        /// </summary>
        private static bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }

        /// <summary>
        /// Returns whether or not the operating system is 64-bit.
        /// </summary>
        private static bool Is64BitOperatingSystem
        {
            get
            {
                if (Is64BitProcess)
                {
                    return true;
                }
                bool isWow64;
                return ModuleContainsFunction("kernel32.dll", "IsWow64Process") && Win32.IsWow64Process(Win32.GetCurrentProcess(), out isWow64) && isWow64;
            }
        }

        /// <summary>
        /// Checks whether or not the specified module contains the specified method.
        /// </summary>
        /// <param name="moduleName">Name of the module to check.</param>
        /// <param name="methodName">Name of the method to check for in the module.</param>
        /// <returns>True if the method was found in the module; false otherwise.</returns>
        private static bool ModuleContainsFunction(string moduleName, string methodName)
        {
            IntPtr hModule = Win32.GetModuleHandle(moduleName);
            if (hModule != IntPtr.Zero)
            {
                return Win32.GetProcAddress(hModule, methodName) != IntPtr.Zero;
            }
            return false;
        }

        /// <summary>
        /// Private class that provides p/invoke access to specific Win32 calls.
        /// </summary>
        private class Win32
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public extern static bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] out bool isWow64);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public extern static IntPtr GetCurrentProcess();
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public extern static IntPtr GetModuleHandle(string moduleName);
            [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
            public extern static IntPtr GetProcAddress(IntPtr hModule, string methodName);
        }
#endif

        /// <summary>
        /// Returns whether or not the operating system is 64-bit.
        /// </summary>
        /// <returns>True = 64-bit, False = 32-bit</returns>
        private static bool Is64Bit()
        {
#if NET_2_0 || NET_3_5
            return Is64BitOperatingSystem;
#elif NET_4_0 || NET_4_5 || NET_4_5_1
            return Environment.Is64BitOperatingSystem;
#endif
        }

        /// <summary>
        /// Gets the bitness of the operating system.
        /// </summary>
        private static int OperatingSystemBitness { get { return Is64Bit() ? 64 : 32; } }

        /// <summary>
        /// Gets the name of the operating system.
        /// </summary>
        private static string OperatingSystemName { get { return Environment.OSVersion.ToString(); } }

        /// <summary>
        /// Gets the version of the current .NET common language runtime environment.
        /// </summary>
        private static string DotNetClrVersion { get { return Environment.Version.ToString().Trim(); } }

        /// <summary>
        /// Gets the version of the current .NET environment.
        /// </summary>
        private static string DotNetVersion 
        {
            get
            {
#if NET_2_0
                return "2.0";
#elif NET_3_5
                return "3.5";
#elif NET_4_0
                return "4.0";
#elif NET_4_5
                return "4.5";
#elif NET_4_5_1
                return "4.5.1";
#endif
            }
        }
    }
}
