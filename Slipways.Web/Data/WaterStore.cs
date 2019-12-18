using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;

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

        public override Water ToClass(
            WaterDto item)
        {
            return new Water(item);
        }

        public override WaterDto ToDto(
            Water item)
        {
            return new WaterDto(item);
        }
    }
}
