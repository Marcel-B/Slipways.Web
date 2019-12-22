using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Data.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IWaterService: ITokenService<WaterDto>
    {
    }

    public class WaterService : TokenService<WaterDto>, IWaterService
    {
        public WaterService(
            HttpClient client,
            IIdentityProviderService tokenService,
            IServiceProvider services,
            IMemoryCache cache,
            ILogger<WaterDto> logger) : base(client, tokenService, services, cache, logger)
        {
            ApiPath = "water";
        }

        public async Task<WaterDto> UpdateWaterAsync(
            WaterDto waterDto)
        {
            var token = await GetTokenAsync();
            if (token == null)
                return null;

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonSerializer.Serialize(waterDto, _jsonOptions);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://data.slipways.de/api/water/{waterDto.Id}"),
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var result = await _client.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                //_logger.LogWarning($"Failed to load update Water {waterDto.Longname}. Status Code: '{(int)result.StatusCode}: {result.ReasonPhrase}' '{jsonContent}'");
                return null;
            }
            var resultObj = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WaterDto>(resultObj, _jsonOptions);
        }
    }
}
