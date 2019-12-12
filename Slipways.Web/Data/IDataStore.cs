using com.b_velop.Slipways.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Data
{
    public interface IDataStore
    {
        Task<HashSet<Slipway>> GetSlipwaysAsync();
        Task<HashSet<Slipway>> AddSlipwayAsync(Slipway slipway, IEnumerable<Guid> extras);
        Task<HashSet<Slipway>> RemoveSlipwayAsync(Guid id);
        Task<HashSet<Water>> GetWatersAsync();
    }
}