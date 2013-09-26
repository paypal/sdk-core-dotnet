using PayPal.Authentication;

namespace PayPal
{
    public interface IAuthenticationStrategy<T, E> where E : ICredential
    {
        T GenerateHeaderStrategy(E e); 
    }
}
