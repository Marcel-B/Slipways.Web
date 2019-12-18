using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;

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

        public override Extra ToClass(
            ExtraDto item)
        {
            return new Extra(item);
        }

        public override ExtraDto ToDto(
            Extra item)
        {
            return new ExtraDto(item);
        }
    }
}
