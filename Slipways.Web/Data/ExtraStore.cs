using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public override async Task<HashSet<Extra>> AddAsync(
            Extra item)
        {
            var result = await InsertAsync(new ExtraDto(item));

            if (result == null)
                return null;

            item.Id = result.Id;
            var cache = await AddToCacheAsync(item);
            return cache;
        }
    }
}
