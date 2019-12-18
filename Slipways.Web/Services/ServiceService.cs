﻿using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Web.Data.Dtos;
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
            IMemoryCache cache,
            IIdentityProviderService tokenService,
            ILogger<ServiceDto> logger) : base(httpClient, tokenService, services, cache, logger)
        {
        }
    }
}
