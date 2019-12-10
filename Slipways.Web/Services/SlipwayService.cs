using com.b_velop.IdentityProvider;
using com.b_velop.IdentityProvider.Model;
using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using GraphQL.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (result == null)
                return new List<T>();
            return result.GetDataFieldAs<IEnumerable<T>>(name);
        }

        public async Task<IEnumerable<WaterDto>> GetWaterAsync()
        {
            try
            {
                var query = @"query {
                          waters {
                            id
                            longname
                            shortname
                          }
                        }";
                return await GetAsync<WaterDto>(query, "waters");
            }
            catch (Exception e)
            {
                _logger.LogError(2222, $"Error occurred while loading Waters from GraphQL endpoint", e);
            }
            return new WaterDto[0];
        }

        public async Task<IEnumerable<SlipwayDto>> GetSlipwaysAsync()
        {
            try
            {
                var query = @"query {
                          slipways {
                            id
                            city
                            name
                            longitude
                            latitude
                            costs
                          }
                        }";
                return await GetAsync<SlipwayDto>(query, "slipways");
            }
            catch (Exception e)
            {
                _logger.LogError(2222, $"Error occurred while request Slipways from GraphQL endpoint", e);
            }
            return new SlipwayDto[0];
        }

        public async Task<IEnumerable<Water>> GetWatersAsync()
        {
            var query = @"query {
                              waters {
                                id
                                longname
                              }
                            }";
            var waters = await GetAsync<Water>(query, "waters");
            return waters.OrderBy(_ => _.Longname);
        }

        public async Task<IEnumerable<ExtraDto>> GetExtrasAsync()
        {
            try
            {
                var query = @"query {
                              extras {
                                id
                                name
                              }
                            }";
                var extras = await GetAsync<ExtraDto>(query, "extras");
                return extras.OrderBy(_ => _.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while request Extras from GraphQL", e);
                return new ExtraDto[0];
            }
        }

        public async Task<SlipwayDto> InsertSlipway(
            SlipwayDto slipway)
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
                    var response = await _httpClient.PostAsync("https://data.slipways.de/api/slipway", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    var result = await content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<SlipwayDto>(result);
                }
                catch (JsonException e)
                {
                    _logger.LogError(1211, $"Error occurred while deserialize response {slipway.Name} {e.StackTrace}", e);
                    return null;
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError(1211, $"Error occurred while post new Slipway {slipway.Name} {e.StackTrace}", e);
                    return null;
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError(1211, $"Error occurred while post new Slipway {slipway.Name} {e.StackTrace}", e);
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(1211, $"Error occurred while post new Slipway {slipway.Name} {e.StackTrace}", e);
                return null;
            }
        }
    }
}
