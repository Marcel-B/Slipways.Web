using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Data.Dtos;
using Microsoft.AspNetCore.Hosting;
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
            IWebHostEnvironment environment,
            IServiceProvider services,
            IMemoryCache cache,
            ILogger<ManufacturerDto> logger) : base(client, tokenService, environment, services, cache, logger)
        {
            ApiPath = "manufacturer";
        }
    }
}
