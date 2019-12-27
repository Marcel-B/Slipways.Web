using com.b_velop.IdentityProvider;
using com.b_velop.IdentityProvider.Model;
using com.b_velop.Slipways.Web.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface ITokenService<DTO> where DTO : class
    {
        Task<DTO> InsertAsync(DTO item);
        Task<DTO> DeleteAsync(Guid id);
        Task<DTO> UpdateAsync(Guid id, DTO item);
    }

    public abstract class TokenService<DTO> : ITokenService<DTO> where DTO : class
    {
        private IServiceProvider _services;
        private IIdentityProviderService _tokenService;
        private IWebHostEnvironment _environment;
        private IMemoryCache _cache;
        private ILogger<DTO> _logger;
        protected JsonSerializerOptions _jsonOptions;
        protected HttpClient _client { get; set; }
        protected const string ApplicationJson = "application/json";
        protected string ApiPath { get; set; }

        protected TokenService(
            HttpClient client,
            IIdentityProviderService tokenService,
            IWebHostEnvironment environment,
            IServiceProvider services,
            IMemoryCache cache,
            ILogger<DTO> logger)
        {
            _client = client;
            _tokenService = tokenService;
            _environment = environment;
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
            var sp = new SecretProvider();
            infoItem.Secret = sp.GetSecret("slipways_web");
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
            if (!await SetHeader())
                return default;
            var url = $"https://data.slipways.de/api/{ApiPath}/{id}";
            if (_environment.IsDevelopment())
                url = $"http://slipways-api:80/api/{ApiPath}/{id}";
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                };

                var result = await _client.SendAsync(request);

                if (!result.IsSuccessStatusCode)
                    return null;

                return await JsonSerializer.DeserializeAsync<DTO>(await result.Content.ReadAsStreamAsync(), _jsonOptions);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(1111, $"Error occurred while deleting '{id}'", e);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(2222, $"Error occurred while deleting '{id}'", e);
            }
            catch (UriFormatException e)
            {
                _logger.LogError(3333, $"Error occurred while deleting '{id}'", e);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(4444, $"Error occurred while deleting '{id}'", e);
            }
            catch (JsonException e)
            {
                _logger.LogError(5555, $"Error occurred while deleting '{id}'", e);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while deleting '{id}'", e);
            }
            return null;
        }

        public async Task<DTO> InsertAsync(
            DTO item)
        {
            if (!await SetHeader())
                return default;

            try
            {
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
            catch (ArgumentNullException e)
            {
                _logger.LogError(1111, $"Error occurred while inserting new item", e);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(2222, $"Error occurred while inserting new item", e);
            }
            catch (UriFormatException e)
            {
                _logger.LogError(3333, $"Error occurred while inserting new item", e);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(4444, $"Error occurred while inserting new item", e);
            }
            catch (JsonException e)
            {
                _logger.LogError(5555, $"Error occurred while inserting new item", e);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while inserting new item", e);
            }
            return null;
        }

        public async Task<DTO> UpdateAsync(
            Guid id,
            DTO item)
        {
            if (!await SetHeader())
                return default;

            try
            {
                var json = JsonSerializer.Serialize(item, _jsonOptions);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://data.slipways.de/api/{ApiPath}/{id}"),
                    Content = new StringContent(json, Encoding.UTF8, ApplicationJson),
                };

                var result = await _client.SendAsync(request);

                if (!result.IsSuccessStatusCode)
                    return null;

                return await JsonSerializer.DeserializeAsync<DTO>(await result.Content.ReadAsStreamAsync(), _jsonOptions);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(1111, $"Error occurred while updating '{id}'", e);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(2222, $"Error occurred while updating '{id}'", e);
            }
            catch (UriFormatException e)
            {
                _logger.LogError(3333, $"Error occurred while updating '{id}'", e);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(4444, $"Error occurred while updating '{id}'", e);
            }
            catch (JsonException e)
            {
                _logger.LogError(5555, $"Error occurred while updating '{id}'", e);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while updating '{id}'", e);
            }
            return null;
        }
    }
}
