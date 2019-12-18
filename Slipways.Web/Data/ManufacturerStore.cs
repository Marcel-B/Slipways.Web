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
    public interface IManufacturerStore : IDataStore<Manufacturer, ManufacturerDto> { }
    public class ManufacturerStore : DataStore<Manufacturer, ManufacturerDto>, IManufacturerStore
    {
        private ILogger<ManufacturerStore> _logger;

        public ManufacturerStore(
            IMemoryCache cache,
            IManufacturerService service,
            IGraphQLService graphQLService,
            ILogger<ManufacturerStore> logger) : base(cache, service, graphQLService)
        {
            _logger = logger;
            Key = Cache.Manufacturers;
            Method = Queries.Manufacturers.Item1;
            Query = Queries.Manufacturers.Item2;
        }

        public override async Task<HashSet<Manufacturer>> AddAsync(
            Manufacturer item)
        {
            var result = await InsertAsync(new ManufacturerDto(item));

            if (result == null)
                return null;

            item.Id = result.Id;
            var cache = await AddToCacheAsync(item);
            return cache;
        }
    }
}
