namespace com.b_velop.Slipways.Web.Infrastructure
{
    public interface ISecretProvider
    {
        string GetSecret(string key);
    }
}