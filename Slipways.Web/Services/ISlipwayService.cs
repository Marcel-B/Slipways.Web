using com.b_velop.Slipways.Web.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface ISlipwayService
    {
        Task<IEnumerable<Slipway>> GetSlipwaysAsync();
        Task<IEnumerable<Water>> GetWatersAsync();
        Task<bool> InsertSlipway(Slipway slipway);
    }
}