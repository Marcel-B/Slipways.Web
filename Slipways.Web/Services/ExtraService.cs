using com.b_velop.Slipways.Data.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public interface IExtraService : ITokenService<ExtraDto> { }

    public class ExtraService : TokenService<ExtraDto>, IExtraService
    {
        public ExtraService(
            HttpClient client,
            IWebHostEnvironment environment,
            ILogger<ExtraService> logger) : base(client, environment, logger)
        {
            ApiPath = "extra";
        }
    }
}
