using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Contracts;
using com.b_velop.Slipways.Web.Infrastructure;
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

        public override Water ConvertToClass(
            WaterDto item)
            => item.ToClass();

        public override WaterDto ConvertToDto(
            Water item)
            => item.ToDto();
    }
}
