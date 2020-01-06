using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Contracts;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;

namespace com.b_velop.Slipways.Web.Data
{
    public interface ISlipwayStore : IDataStore<Slipway, SlipwayDto> { }

    public class SlipwayStore : DataStore<Slipway, SlipwayDto>, ISlipwayStore
    {
        public SlipwayStore(
            IMemoryCache cache,
            ISlipwayService service,
            IGraphQLService graphQLService) : base(cache, service, graphQLService)
        {
            Key = Cache.Slipways;
            Method = Queries.Slipways.Item1;
            Query = Queries.Slipways.Item2;
        }

        public override Slipway ConvertToClass(
            SlipwayDto item)
            => item.ToClass();

        public override SlipwayDto ConvertToDto(
            Slipway item)
            => item.ToDto();
    }
}
