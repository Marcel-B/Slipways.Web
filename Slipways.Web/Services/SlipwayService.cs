using com.b_velop.IdentityProvider;
using com.b_velop.IdentityProvider.Model;
using com.b_velop.Slipways.Web.Data.Models;
using GraphQL.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public class SlipwayService : ISlipwayService
    {
        private readonly HttpClient _httpClient;
        private readonly GraphQLClient _client;
        private readonly IServiceProvider _services;
        private readonly IMemoryCache _cache;
        private readonly IIdentityProviderService _tokenService;
        private readonly ILogger<SlipwayService> _logger;

        public SlipwayService(
            HttpClient httpClient,
            GraphQLClient client,
            IServiceProvider services,
            IMemoryCache cache,
            IIdentityProviderService tokenService,
            ILogger<SlipwayService> logger)
        {
            _httpClient = httpClient;
            _client = client;
            _services = services;
            _cache = cache;
            _tokenService = tokenService;
            _logger = logger;
        }

        private async Task<Token> RequestTokenAsync()
        {
            var infoItem = _services.GetRequiredService<InfoItem>();
            var token = await _tokenService.GetTokenAsync(infoItem);
            return token;
        }
        private async Task<string> GetTokenAsync()
        {
            Token token = null;
            if (_cache.TryGetValue("last", out DateTime time))
            {
                if (time < DateTime.Now)
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
        private async Task<IEnumerable<T>> GetAsync<T>(
            string query,
            string name)
        {
            var result = await _client.GetQueryAsync(query);
            return result.GetDataFieldAs<IEnumerable<T>>(name);
        }

        public async Task<IEnumerable<Slipway>> GetSlipwaysAsync()
        {
            var query = @"query {
                          slipways {
                            id
                            city
                            name
                            longitude
                            latitude
                          }
                        }";
            return await GetAsync<Slipway>(query, "slipways");
        }

        public async Task<IEnumerable<Water>> GetWatersAsync()
        {
            var query = @"query {
                              waters {
                                id
                                longname
                              }
                            }";
            return await GetAsync<Water>(query, "waters");
        }

        public async Task<bool> InsertSlipway(
            Slipway slipway)
        {
            try
            {
                var token = await GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var modelJson = JsonSerializer.Serialize(slipway, options);

                var content = new StringContent(modelJson, Encoding.UTF8, "application/json");
                try
                {
                    var response = await _httpClient.PostAsync("https://slipways.de/api/slipway", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }
                    return true;
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError(1211, $"Error occurred while post new Slipway {slipway.Name} {e.StackTrace}", e);
                    return false;
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError(1211, $"Error occurred while post new Slipway {slipway.Name} {e.StackTrace}", e);
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(1211, $"Error occurred while post new Slipway {slipway.Name} {e.StackTrace}", e);
                return false;
            }
        }
    }
}
