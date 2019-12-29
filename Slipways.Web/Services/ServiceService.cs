using com.b_velop.Slipways.Data.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public class ServiceService : TokenService<ServiceDto>, IServiceService
    {
        public ServiceService(
            HttpClient client,
            IWebHostEnvironment environment,
            ILogger<ServiceService> logger) : base(client, environment, logger)
        {
            ApiPath = "service";
        }
    }
}
