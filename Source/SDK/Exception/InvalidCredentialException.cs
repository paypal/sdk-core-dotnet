namespace PayPal.Exception
{
    public class InvalidCredentialException : System.Exception
    {
        public InvalidCredentialException(string message) : base(message) { }
    }
}
