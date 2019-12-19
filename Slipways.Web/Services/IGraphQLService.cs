using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IGraphQLService
    {
        Task<T> GetValuesAsync<T>(string method, string query);
    }
}