using com.b_velop.Slipways.Data.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IManufacturerService : ITokenService<ManufacturerDto>
    {
    }

    public class ManufacturerService : TokenService<ManufacturerDto>, IManufacturerService
    {
        public ManufacturerService(
            HttpClient client,
            IWebHostEnvironment environment,
            ILogger<ManufacturerService> logger) : base(client, environment, logger)
        {
            ApiPath = "manufacturer";
        }
    }
}
