using com.b_velop.Slipways.Web.Data.Models;
using GraphQL.Client;
using GraphQL.Common.Request;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public class SlipwayService : ISlipwayService
    {
        private readonly GraphQLClient _client;
        private readonly ILogger<SlipwayService> _logger;

        public SlipwayService(
            GraphQLClient client,
            ILogger<SlipwayService> logger)
        {
            _client = client;
            _logger = logger;
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
            var query = @"mutation createSlipway($slipway: SlipwayInput!) {
                              createSlipway(slipway: $slipway) {
                                id
                              }
                            }";
            var request = new GraphQLRequest
            {
                OperationName = "createSlipway",
                Query = query,
                Variables = new
                {
                    slipway = new
                    {
                        slipway.Name,
                        slipway.Latitude,
                        slipway.Longitude,
                        slipway.Costs,
                        WaterFk = slipway.Water,
                        slipway.Rating,
                        slipway.City,
                        slipway.Street,
                        slipway.Postalcode
                    }
                }
            };
            var result = await _client.PostAsync(request);
            return result.Errors == null;
        }
    }
}
