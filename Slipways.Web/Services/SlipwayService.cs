using com.b_velop.Slipways.Data.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public class SlipwayService : TokenService<SlipwayDto>, ISlipwayService
    {
        public SlipwayService(
            HttpClient client,
            IWebHostEnvironment environment,
            ILogger<SlipwayService> logger) : base(client, environment, logger)
        {
            ApiPath = "slipways";
        }
    }
}
