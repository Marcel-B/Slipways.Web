using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Contracts;
using com.b_velop.Slipways.Web.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace com.b_velop.Slipways.Web.Data
{
    public class PortStore : DataStore<Port, PortDto>, IPortStore
    {
        public PortStore(
            IMemoryCache cache,
            ITokenService<PortDto> service,
            IGraphQLService graphQLService) : base(cache, service, graphQLService)
        {
            Key = Cache.Ports;
            Method = Queries.Ports.Item1;
            Query = Queries.Ports.Item2;
        }

        public override Port ConvertToClass(
            PortDto item)
            => item.ToClass();

        public override PortDto ConvertToDto(
            Port item)
            => item.ToDto();
    }
}
