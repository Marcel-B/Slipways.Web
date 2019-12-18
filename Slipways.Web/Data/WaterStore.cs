using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Data
{
    public interface IWaterStore : IDataStore<Water, WaterDto> { }
    public class WaterStore : DataStore<Water, WaterDto>, IWaterStore
    {
        public WaterStore(
            IMemoryCache cache,
            IWaterService service,
            IGraphQLService graphQLService) : base(cache, service, graphQLService)
        {
            Key = Cache.Waters;
            Method = Queries.Waters.Item1;
            Query = Queries.Waters.Item2;
        }

        public override async Task<HashSet<Water>> AddAsync(
            Water item)
        {
            var result = await InsertAsync(new WaterDto(item));

            if (result == null)
                return null;

            item.Id = result.Id;
            var cache = await AddToCacheAsync(item);

            return cache;
        }
    }
}
