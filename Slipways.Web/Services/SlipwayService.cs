using com.b_velop.Slipways.Web.Data.Models;
using GraphQL.Client;
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

        public async Task<IEnumerable<Slipway>> GetSlipwaysAsync()
        {
            var query = @"query {
                          slipways {
                            id
                            city
                            name
                            longitude
                            latitude
                            water {
                              id
                              longname
                              shortname
                            }
                          }
                        }";
            var result = await _client.GetQueryAsync(query);
            return result.GetDataFieldAs<IEnumerable<Slipway>>("slipways");
        }
    }
}
