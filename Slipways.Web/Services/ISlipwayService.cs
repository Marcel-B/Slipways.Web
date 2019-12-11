using com.b_velop.Slipways.Web.Data.Dtos;
using System;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface ISlipwayService
    {
        Task<SlipwayDto> InsertSlipway(SlipwayDto slipway);
        Task<SlipwayDto> DeleteSlipwayAsync(Guid id);
    }
}