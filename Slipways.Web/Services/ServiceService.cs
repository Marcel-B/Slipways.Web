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
    public class ServiceService : TokenService, IServiceService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(
            HttpClient httpClient,
            IServiceProvider services,
            IMemoryCache cache,
            IIdentityProviderService tokenService,
            ILogger<ServiceService> logger) : base(tokenService, services, cache)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ServiceDto> InsertAsync(
            ServiceDto serviceDto)
        {
            try
            {
                var token = await GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var modelJson = JsonSerializer.Serialize(serviceDto, _jsonOptions);
                var content = new StringContent(modelJson, Encoding.UTF8, "application/json");

                try
                {
                    var response = await _httpClient.PostAsync("https://data.slipways.de/api/service", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ServiceDto>(result, _jsonOptions);
                }
                catch (JsonException e)
                {
                    _logger.LogError(1211, $"Error occurred while deserialize response {serviceDto.Name} {e.StackTrace}", e);
                    return null;
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError(1211, $"Error occurred while post new Service {serviceDto.Name} {e.StackTrace}", e);
                    return null;
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError(1211, $"Error occurred while post new Service {serviceDto.Name} {e.StackTrace}", e);
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(1211, $"Error occurred while post new Service {serviceDto.Name} {e.StackTrace}", e);
                return null;
            }
        }
    }
}
