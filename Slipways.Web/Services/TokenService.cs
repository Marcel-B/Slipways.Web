using com.b_velop.IdentityProvider;
using com.b_velop.IdentityProvider.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public abstract class TokenService
    {
        private IServiceProvider _services;
        private IIdentityProviderService _tokenService;
        private IMemoryCache _cache;
        protected JsonSerializerOptions _jsonOptions;

        protected TokenService(
            IIdentityProviderService tokenService,
            IServiceProvider services,
            IMemoryCache cache)
        {
            _tokenService = tokenService;
            _services = services;
            _cache = cache;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                IgnoreNullValues = true
            };
        }

        protected async Task<Token> RequestTokenAsync()
        {
            var infoItem = _services.GetRequiredService<InfoItem>();
            var token = await _tokenService.GetTokenAsync(infoItem);
            return token;
        }
        protected async Task<string> GetTokenAsync()
        {
            Token token = null;
            if (_cache.TryGetValue("last", out DateTime time))
            {
                if (time > DateTime.Now)
                {
                    if (_cache.TryGetValue("token", out token))
                    {
                        // Valid token
                        return token.AccessToken;
                    }
                }
            }
            token = await RequestTokenAsync();

            if (token == null)
                return null;

            _cache.Set("last", DateTime.Now.AddSeconds(token.ExpiresIn));
            _cache.Set("token", token);
            return token.AccessToken;
        }
    }
}
