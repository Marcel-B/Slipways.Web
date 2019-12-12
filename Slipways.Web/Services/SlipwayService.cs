using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Web.Data.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public class SlipwayService : TokenService, ISlipwayService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SlipwayService> _logger;

        public SlipwayService(
            HttpClient httpClient,
            IServiceProvider services,
            IMemoryCache cache,
            IIdentityProviderService tokenService,
            ILogger<SlipwayService> logger) : base(tokenService, services, cache)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

    
        public async Task<SlipwayDto> InsertSlipway(
            SlipwayDto slipway)
        {
            try
            {
                var token = await GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



                var modelJson = JsonSerializer.Serialize(slipway, _jsonOptions);
                var content = new StringContent(modelJson, Encoding.UTF8, "application/json");

                try
                {
                    var response = await _httpClient.PostAsync("https://data.slipways.de/api/slipway", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<SlipwayDto>(result, _jsonOptions);
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

        public async Task<SlipwayDto> DeleteSlipwayAsync(
            Guid id)
        {
            var token = await GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://data.slipways.de/api/slipway/{id}"),
            };

            var result = await _httpClient.SendAsync(request);
            if (!result.IsSuccessStatusCode)
                return null;
            var resultObj = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SlipwayDto>(resultObj, _jsonOptions);
        }
    }
}
