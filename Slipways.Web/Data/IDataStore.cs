using com.b_velop.Slipways.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Data
{
    public interface IDataStore
    {
        Task<HashSet<Slipway>> GetSlipwaysAsync();
        Task<HashSet<Slipway>> AddSlipwayAsync(Slipway slipway);
        Task<HashSet<Slipway>> RemoveSlipwayAsync(Guid id);
        Task<HashSet<Water>> GetWatersAsync();
        Task<HashSet<Water>> AddWaterAsync(Water water);
        Task<HashSet<Water>> RemoveWaterAsync(Guid id);
        Task<HashSet<Service>> GetServicesAsync();
    }
}