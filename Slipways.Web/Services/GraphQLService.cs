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

        //public async Task<IEnumerable<WaterDto>> GetWaterAsync()
        //{
        //    try
        //    {
        //        var query = @"query {
        //                  waters {
        //                    id
        //                    longname
        //                    shortname
        //                  }
        //                }";
        //        var waterDtos = await GetAsync<WaterDto>(query, "waters");
        //        return waterDtos.OrderBy(_ => _.Longname);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(2222, $"Error occurred while loading Waters from GraphQL endpoint", e);
        //    }
        //    return new WaterDto[0];
        //}

        //public async Task<IEnumerable<WaterDto>> GetWatersAsync()
        //{
        //    var query = @"query {
        //                      waters {
        //                        id
        //                        longname
        //                        shortname
        //                      }
        //                    }";
        //    var waters = await GetAsync<WaterDto>(query, "waters");
        //    return waters.OrderBy(_ => _.Longname);
        //}

        //public async Task<IEnumerable<ExtraDto>> GetExtrasAsync()
        //{
        //    try
        //    {
        //        var query = @"query {
        //                      extras {
        //                        id
        //                        name
        //                      }
        //                    }";
        //        var extras = await GetAsync<ExtraDto>(query, "extras");
        //        return extras.OrderBy(_ => _.Name);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(6666, $"Error occurred while request Extras from GraphQL", e);
        //        return new ExtraDto[0];
        //    }
        //}

 

      

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
