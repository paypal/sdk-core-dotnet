
namespace PayPal.Manager
{
    public interface IAccount
    {
        string APIUsername { get; }
        string APIPassword { get; }
        string APISignature { get; }
        string ApplicationId { get; }
        string APICertificate { get; }
        string PrivateKeyPassword { get; }
        string SignatureSubject { get; }
        string CertificateSubject { get; }
    }
}
