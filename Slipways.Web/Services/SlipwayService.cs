﻿using com.b_velop.IdentityProvider;
using com.b_velop.Slipways.Web.Data.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace com.b_velop.Slipways.Web.Services
{
    public class SlipwayService : TokenService<SlipwayDto>, ISlipwayService
    {
        public SlipwayService(
            HttpClient httpClient,
            IServiceProvider services,
            IMemoryCache cache,
            IIdentityProviderService tokenService,
            ILogger<SlipwayDto> logger) : base(httpClient, tokenService, services, cache, logger)
        {
        }
    }
}
