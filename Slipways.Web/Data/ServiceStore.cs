using AutoMapper;
using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Contracts;
using com.b_velop.Slipways.Web.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace com.b_velop.Slipways.Web.Data
{
    public class ServiceStore : DataStore<Service, ServiceDto>, IServiceStore
    {
        public ServiceStore(
            IMemoryCache cache,
            IMapper mapper,
            IServiceService service,
            IGraphQLService graphQLService,
            ILogger<ServiceStore> logger) : base(cache, mapper, service, graphQLService)
        {
            Key = Cache.Services;
            Query = Queries.Services.Item2;
            Method = Queries.Services.Item1;
        }

        public override IEnumerable<Service> Sort(IEnumerable<Service> set)
            => set.OrderBy(_ => _.Name);

    }
}
