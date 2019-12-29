﻿using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Data.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public class ServiceService : TokenService<ServiceDto>, IServiceService
    {
        public ServiceService(
            HttpClient httpClient,
            IServiceProvider services,
            IWebHostEnvironment environment,
            IMemoryCache cache,
            IIdentityProviderService tokenService,
            ILogger<ServiceService> logger) : base(httpClient, tokenService, environment, services, cache, logger)
        {
            ApiPath = "service";
        }
    }
}
