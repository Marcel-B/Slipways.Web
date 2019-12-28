using GraphQL.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public class GraphQLService : IGraphQLService
    {
        private ILogger<GraphQLService> _logger;
        private GraphQLClient _client;

        public GraphQLService(
            GraphQLClient client,
            ILogger<GraphQLService> logger)
        {
            _logger = logger;
            _client = client;
        }

        private async Task<T> GetAsync<T>(
        string query,
        string name)
        {
            var result = await _client.GetQueryAsync(query);

            if (result == null)
                return default;

            return result.GetDataFieldAs<T>(name);
        }

        public async Task<T> GetValuesAsync<T>(
            string method,
            string query)
        {
            try
            {
                var result = await GetAsync<T>(query, method);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while calling GraphQL\nQuery: '{query}'\nMethod: '{method}'", e);
                return default;
            }
        }
    }
}
