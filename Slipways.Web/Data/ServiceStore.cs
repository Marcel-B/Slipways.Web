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
    public interface IServiceStore : IDataStore<Service, ServiceDto> { }
    public class ServiceStore : DataStore<Service, ServiceDto>, IServiceStore
    {
        public ServiceStore(
            IMemoryCache cache,
            IServiceService service,
            IGraphQLService graphQLService,
            ILogger<ServiceStore> logger) : base(cache, service, graphQLService)
        {
            Key = Cache.Services;
            Query = Queries.Services.Item2;
            Method = Queries.Services.Item1;
        }

        public override async Task<HashSet<Service>> AddAsync(
            Service item)
        {
            var result = await InsertAsync(new ServiceDto(item));

            if (result == null)
                return null;

            item.Id = result.Id;
            var cache = await AddToCacheAsync(item);
            return cache;
        }
    }
}
