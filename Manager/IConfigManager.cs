
namespace PayPal.Manager
{
    public interface IConfigManager
    {
        string GetProperty(string key);
        IAccount GetAccount(string apiUserName);
        IAccount GetAccount(int index);
    }
}
