using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using com.b_velop.Slipways.Data.Helper;

namespace com.b_velop.Slipways.Web.Data
{
    public interface IExtraStore : IDataStore<Extra, ExtraDto> { }

    public class ExtraStore : DataStore<Extra, ExtraDto>, IExtraStore
    {
        public ExtraStore(
            IMemoryCache cache,
            IExtraService service,
            IGraphQLService graphQLService) : base(cache, service, graphQLService)
        {
            Key = Cache.Extras;
            Query = Queries.Extras.Item2;
            Method = Queries.Extras.Item1;
        }

        public override Extra ConvertToClass(
            ExtraDto item)
            => item.ToClass();

        public override ExtraDto ConvertToDto(
            Extra item)
            => item.ToDto();
    }
}
