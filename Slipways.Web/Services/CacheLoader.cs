﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Services
{
    public class CacheLoader : IHostedService, IDisposable
    {
        private readonly ILogger<CacheLoader> _logger;
        private Timer _timer;

        public CacheLoader(
        IServiceProvider services,
        ILogger<CacheLoader> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CacheLoader Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void DoWork(
            object state)
        {
            //try
            //{
            //    using var scope = Services.CreateScope();
            //    var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            //    var graphQLService = scope.ServiceProvider.GetRequiredService<IGraphQLService>();

            //    var waters = await graphQLService.GetValuesAsync<IEnumerable<WaterDto>>(Queries.Waters.Item1, Queries.Waters.Item2);
            //    if (waters != null)
            //    {
            //        var whs = waters.ToHashSet();
            //        cache.Set(Cache.Waters, whs);
            //    }

            //    var slipways = await graphQLService.GetValuesAsync<IEnumerable<SlipwayDto>>(Queries.Slipways.Item1, Queries.Slipways.Item2);
            //    if (slipways != null)
            //    {
            //        var shs = slipways.ToHashSet();
            //        cache.Set(Cache.Slipways, shs);
            //    }

            //    var services = await graphQLService.GetValuesAsync<IEnumerable<ServiceDto>>(Queries.Services.Item1, Queries.Services.Item2);
            //    if (services != null)
            //    {
            //        var sehs = services.ToHashSet();
            //        cache.Set(Cache.Services, sehs);
            //    }
            //}catch(Exception e)
            //{
            //    //_logger.LogError(6666, "Error occurred while reload cache", e);
            //}
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}