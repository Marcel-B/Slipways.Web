using com.b_velop.Slipways.Web.Data.Dtos;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IServiceService
    {
        Task<ServiceDto> InsertAsync(ServiceDto serviceDto);
    }
}
