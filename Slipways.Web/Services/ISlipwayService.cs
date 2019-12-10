using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface ISlipwayService
    {
        Task<IEnumerable<SlipwayDto>> GetSlipwaysAsync();
        Task<IEnumerable<Water>> GetWatersAsync();
        Task<IEnumerable<ExtraDto>> GetExtrasAsync();
        Task<SlipwayDto> InsertSlipway(SlipwayDto slipway);
        Task<IEnumerable<WaterDto>> GetWaterAsync();
    }
}