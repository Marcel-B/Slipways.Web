using com.b_velop.IdentityProvider;
using com.b_velop.IdentityProvider.Model;
using com.b_velop.Slipways.Web.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface ITokenService<DTO> where DTO : class, IEntity
    {
        Task<DTO> InsertAsync(DTO item);
        Task<DTO> DeleteAsync(Guid id);
    }

    public abstract class TokenService<DTO> : ITokenService<DTO> where DTO : class, IEntity
    {
        private IServiceProvider _services;
        private IIdentityProviderService _tokenService;
        private IMemoryCache _cache;
        private ILogger<DTO> _logger;
        protected JsonSerializerOptions _jsonOptions;
        protected HttpClient _client { get; set; }
        protected const string ApplicationJson = "application/json";

        protected TokenService(
            HttpClient client,
            IIdentityProviderService tokenService,
            IServiceProvider services,
            IMemoryCache cache,
            ILogger<DTO> logger)
        {
            _client = client;
            _tokenService = tokenService;
            _services = services;
            _cache = cache;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                IgnoreNullValues = true
            };
        }

        protected async Task<Token> RequestTokenAsync()
        {
            var infoItem = _services.GetRequiredService<InfoItem>();
            var token = await _tokenService.GetTokenAsync(infoItem);
            if (token == null)
            {
                var info = string.IsNullOrWhiteSpace(infoItem.Secret) ? "No Secret" : "Secret is not null";
                _logger.LogError(6666, $"Error occurred while request token.\nClientID: '{infoItem.ClientId}'\nScope: '{infoItem.Scope}'\nUrl: '{infoItem.Url}'\nSecret: '{info}'");
                return null;
            }
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
            {
                return null;
            }

            _cache.Set("last", DateTime.Now.AddSeconds(token.ExpiresIn));
            _cache.Set("token", token);
            return token.AccessToken;
        }

        public async Task<bool> SetHeader()
        {
            var token = await GetTokenAsync();
            if (token == null)
                return false;
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        public async Task<DTO> DeleteAsync(
            Guid id)
        {
            if (await SetHeader())
                return default;

            var path = typeof(DTO).GetType().Name switch
            {
                "SlipwayDto" => "slipway",
                "ExtraDto" => "extra",
                _ => ""
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://data.slipways.de/api/{path}/{id}"),
            };

            var result = await _client.SendAsync(request);

            if (!result.IsSuccessStatusCode)
                return null;

            return await JsonSerializer.DeserializeAsync<DTO>(await result.Content.ReadAsStreamAsync(), _jsonOptions);
        }

        public async Task<DTO> InsertAsync(
            DTO item)
        {
            if (await SetHeader())
                return default;

            var json = JsonSerializer.Serialize(item, _jsonOptions);

            var content = new StringContent(json, Encoding.UTF8, ApplicationJson);

            var response = await _client.PostAsync("", content);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var responseContent = await response.Content.ReadAsStreamAsync();
            var obj = await JsonSerializer.DeserializeAsync<DTO>(responseContent);
            return obj;
        }
    }
}
