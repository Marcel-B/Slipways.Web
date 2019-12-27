using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

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

        public override Manufacturer ConvertToClass(
            ManufacturerDto item)
            => item.ToClass();

        public override ManufacturerDto ConvertToDto(
            Manufacturer item)
            => item.ToDto();
    }
}
