using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Web.Data.Dtos;
using Microsoft.Extensions.Caching.Memory;
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
    public interface IWaterService
    {
        Task<WaterDto> InsertAsync(WaterDto water);
        Task<IEnumerable<WaterDto>> SelectAllWatersAsync();
        Task<WaterDto> DeleteWaterAsync(Guid uuid);
        Task<WaterDto> UpdateWaterAsync(WaterDto waterDto);
    }

    public class WaterService : TokenService, IWaterService
    {
        private HttpClient _client;
        private ILogger<WaterService> _logger;

        public WaterService(
            HttpClient client,
            IIdentityProviderService tokenService,
            IServiceProvider services,
            IMemoryCache cache,
            ILogger<WaterService> logger) : base(tokenService, services, cache)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<WaterDto> DeleteWaterAsync(
            Guid id)
        {
            var token = await GetTokenAsync();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://data.slipways.de/api/water/{id}"),
            };

            var result = await _client.SendAsync(request);
            if (!result.IsSuccessStatusCode)
                return null;
            var resultObj = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WaterDto>(resultObj, _jsonOptions);
        }

        public async Task<WaterDto> InsertAsync(
            WaterDto water)
        {
            var token = await GetTokenAsync();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(water, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await _client.PostAsync("", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    var result = await JsonSerializer.DeserializeAsync<WaterDto>(await responseMessage.Content.ReadAsStreamAsync(), _jsonOptions);
                    return result;
                }
                catch (JsonException e)
                {
                    _logger.LogError(6666, $"Error occurred while deserializen WaterDto", e);
                }
            }
            _logger.LogError(5555, $"Error occurred while insert Water '{water.Longname} - {water.Shortname}'");
            return null;
        }

        public async Task<IEnumerable<WaterDto>> SelectAllWatersAsync()
        {
            var token = await GetTokenAsync();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await _client.GetAsync("");
            if (responseMessage.IsSuccessStatusCode)
            {
                var waterDtos = await JsonSerializer.DeserializeAsync<IEnumerable<WaterDto>>(await responseMessage.Content.ReadAsStreamAsync(), _jsonOptions);
                return waterDtos;
            }
            return null;
        }

        public async Task<WaterDto> UpdateWaterAsync(
            WaterDto waterDto)
        {
            var token = await GetTokenAsync();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonSerializer.Serialize(waterDto, _jsonOptions);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://data.slipways.de/api/water/{waterDto.Id}"),
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var result = await _client.SendAsync(request);

            if (!result.IsSuccessStatusCode)
                return null;

            var resultObj = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WaterDto>(resultObj, _jsonOptions);
        }
    }
}
