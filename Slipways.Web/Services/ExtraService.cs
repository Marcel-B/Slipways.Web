using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Web.Data.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IExtraService : ITokenService<ExtraDto> { }
    public class ExtraService : TokenService<ExtraDto>, IExtraService
    {
        public ExtraService(
            HttpClient client,
            IIdentityProviderService tokenService, 
            IServiceProvider services, 
            IMemoryCache cache, 
            ILogger<ExtraDto> logger) : base(client, tokenService, services, cache, logger)
        {
        }
    }
}
