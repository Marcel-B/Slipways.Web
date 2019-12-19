using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

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

        public override Service ToClass(
            ServiceDto item)
        {
            return new Service(item);
        }

        public override ServiceDto ToDto(
            Service item)
        {
            return new ServiceDto(item);
        }
    }
}
