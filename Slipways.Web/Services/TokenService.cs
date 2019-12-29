using Microsoft.AspNetCore.Hosting;
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
        private IWebHostEnvironment _environment;
        private ILogger<TokenService<DTO>> _logger;
        protected JsonSerializerOptions _jsonOptions;
        protected HttpClient _client { get; set; }
        protected const string ApplicationJson = "application/json";
        protected string ApiPath { get; set; }

        protected TokenService(
            HttpClient client,
            IWebHostEnvironment environment,
            ILogger<TokenService<DTO>> logger)
        {
            _client = client;
            _environment = environment;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                IgnoreNullValues = true
            };
        }

        public async Task<DTO> DeleteAsync(
            Guid id)
        {
            var url = $"http://slipways-api:8095/api/{ApiPath}/{id}";
            if (_environment.IsDevelopment())
                url = $"http://slipways-api:8095/api/{ApiPath}/{id}";
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
            //if (!await SetHeader())
            //    return default;
            string json = string.Empty;
            try
            {
                json = JsonSerializer.Serialize(item, _jsonOptions);

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
                _logger.LogError(1111, $"Error occurred while inserting new item\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(2222, $"Error occurred while inserting new item\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (UriFormatException e)
            {
                _logger.LogError(3333, $"Error occurred while inserting new item\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(4444, $"Error occurred while inserting new item\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (JsonException e)
            {
                _logger.LogError(5555, $"Error occurred while inserting new item\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while inserting new item\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            return null;
        }

        public async Task<DTO> UpdateAsync(
            Guid id,
            DTO item)
        {
            var json = string.Empty;
            try
            {
                json = JsonSerializer.Serialize(item, _jsonOptions);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"http://slipways-api:8095/api/{ApiPath}/{id}"),
                    Content = new StringContent(json, Encoding.UTF8, ApplicationJson),
                };

                var result = await _client.SendAsync(request);

                if (!result.IsSuccessStatusCode)
                    return null;

                return await JsonSerializer.DeserializeAsync<DTO>(await result.Content.ReadAsStreamAsync(), _jsonOptions);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(1111, $"Error occurred while updating '{id}'\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(2222, $"Error occurred while updating '{id}'\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (UriFormatException e)
            {
                _logger.LogError(3333, $"Error occurred while updating '{id}'\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(4444, $"Error occurred while updating '{id}'\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (JsonException e)
            {
                _logger.LogError(5555, $"Error occurred while updating '{id}'\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while updating '{id}'\nItem: '{json}'\nUrl: {_client.BaseAddress}", e);
            }
            return null;
        }
    }
}
