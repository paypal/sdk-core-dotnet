using System;

namespace PayPal.Authentication
{
    public class SubjectAuthorization : IThirdPartyAuthorization
    {
        /// <summary>
        /// Subject information
        /// </summary>
        private string sub;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sub"></param>
        public SubjectAuthorization(string sub) : base()
        {
            if (string.IsNullOrEmpty(sub))
            {
                throw new ArgumentException("SubjectAuthorization arguments cannot be null or empty");
            }
            this.sub = sub;
        }

        /// <summary>
        /// Gets the subject
        /// </summary>
        public string Subject
        {
            get
            {
                return sub;
            }
        }
    }
}
