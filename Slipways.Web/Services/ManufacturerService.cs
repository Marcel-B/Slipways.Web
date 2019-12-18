using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Web.Data.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IManufacturerService : ITokenService<ManufacturerDto>
    {
    }

    public class ManufacturerService : TokenService<ManufacturerDto>, IManufacturerService
    {
        public ManufacturerService(
            HttpClient client,
            IIdentityProviderService tokenService,
            IServiceProvider services,
            IMemoryCache cache,
            ILogger<ManufacturerDto> logger) : base(client, tokenService, services, cache, logger)
        {
        }
    }
}
