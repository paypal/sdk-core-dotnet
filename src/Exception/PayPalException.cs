/* NuGet Install
 * Visual Studio 2008
    * Install log4net -OutputDirectory .\packages
    * Add reference from the folder "net35-full"
 * Visual Studio 2010 or higher
    * Install-Package log4net
    * Reference is auto-added 
*/
using log4net;

namespace PayPal.Exception
{
    public class PayPalException : System.Exception
    {
        /// <summary>
        /// Logs output statements, errors, debug info to a text file    
        /// </summary>
        private static readonly ILog Logger = LogManagerWrapper.GetLogger(typeof(PayPalException));

        /// <summary>
		/// Represents application configuration errors 
		/// </summary>
        public PayPalException() : base() { }
        
		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public PayPalException(string message): base(message)
		{
			if (Logger.IsErrorEnabled)
			{
				Logger.Error(message, this);
			}
		}

		/// <summary>
		/// Represents errors that occur during application execution
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		/// <param name="cause">The exception that is the cause of the current exception</param>
        public PayPalException(string message, System.Exception cause) : base(message, cause)
		{
			if (Logger.IsErrorEnabled) 
			{
				Logger.Error(message, this);
			}
		}
    }
}
