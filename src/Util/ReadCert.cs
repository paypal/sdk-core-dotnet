using System.IO;

namespace PayPal.Util
{
    class ReadCert
    {
        private byte[] Certificate = null;
        private string FilePath = string.Empty;
        private FileStream fileStrm = null;

        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public ReadCert() { }

        /// <summary>
        /// To read the certificate
        /// </summary>
        /// <param name="certpath"></param>
        /// <returns></returns>
        public byte[] ReadCertificate(string certificatePath)
        {
            ///loading the certificate file into profile.
            fileStrm = new FileStream(certificatePath, FileMode.Open, FileAccess.Read);
            Certificate = new byte[fileStrm.Length];
            fileStrm.Read(Certificate, 0, int.Parse(fileStrm.Length.ToString()));
            fileStrm.Close();
            return Certificate;
        }
    }
}
