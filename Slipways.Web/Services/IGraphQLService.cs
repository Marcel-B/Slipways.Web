using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IGraphQLService
    {
        Task<IEnumerable<ExtraDto>> GetExtrasAsync();
        Task<IEnumerable<SlipwayDto>> GetSlipwaysAsync();
        Task<IEnumerable<WaterDto>> GetWaterAsync();
        Task<IEnumerable<WaterDto>> GetWatersAsync();
    }
}