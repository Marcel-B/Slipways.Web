using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using GraphQL.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Services
{
    public class GraphQLService : TokenService, IGraphQLService
    {
        private ILogger<GraphQLService> _logger;
        private GraphQLClient _client;

        public GraphQLService(
            GraphQLClient client,
            IIdentityProviderService tokenService,
            IServiceProvider services,
            ILogger<GraphQLService> logger,
            IMemoryCache cache) : base(tokenService, services, cache)
        {
            _logger = logger;
            _client = client;
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
                var waterDtos = await GetAsync<WaterDto>(query, "waters");
                return waterDtos.OrderBy(_ => _.Longname);
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
                            street
                            postalcode
                            water {
                              id
                              longname
                              shortname
                            }
                            extras { 
                                id
                                name
                            }
                          }
                        }";
                var slipwayDtos =  await GetAsync<SlipwayDto>(query, "slipways");
                return slipwayDtos.OrderBy(_ => _.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(2222, $"Error occurred while request Slipways from GraphQL endpoint", e);
            }
            return new SlipwayDto[0];
        }

        public async Task<IEnumerable<WaterDto>> GetWatersAsync()
        {
            var query = @"query {
                              waters {
                                id
                                longname
                                shortname
                              }
                            }";
            var waters = await GetAsync<WaterDto>(query, "waters");
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

        public async Task<IEnumerable<ServiceDto>> GetServicesAsync()
        {
            try
            {
                var query = @"query {
                              services {
                                id
                                name
                                city
                                latitude
                                longitude
                                postalcode
                                phone
                                email
                                url
                                street
                                manufacturers {
                                    id
                                    name
                                }
                              }
                            }";
                var services = await GetAsync<ServiceDto>(query, "services");
                return services.OrderBy(_ => _.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while request Services from GraphQL", e);
                return new ServiceDto[0];
            }
        }

        public async Task<IEnumerable<ManufacturerDto>> GetManufacturersAsync()
        {
            try
            {
                var query = @"query {
                              manufacturers {
                                id
                                name
                              }
                            }";
                var manufacturerDtos = await GetAsync<ManufacturerDto>(query, "manufacturers");
                return manufacturerDtos.OrderBy(_ => _.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(6666, $"Error occurred while request Manufacturers from GraphQL", e);
                return new ManufacturerDto[0];
            }
        }
    }
}
