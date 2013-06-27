using System;

namespace PayPal.Authentication
{
    public class SubjectAuthorization : IThirdPartyAuthorization
    {
        /// <summary>
        /// Subject information
        /// </summary>
        private string Sub;
           
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject"></param>
        public SubjectAuthorization(string subjct) : base()
        {
            if (string.IsNullOrEmpty(subjct))
            {
                throw new ArgumentException("SubjectAuthorization arguments cannot be null or empty");
            }
            this.Sub = subjct;
        }

        /// <summary>
        /// Gets the subject
        /// </summary>
        public string Subject
        {
            get
            {
                return Sub;
            }
        }
    }
}
