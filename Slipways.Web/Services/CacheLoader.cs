using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
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

        private async void DoWork(object state)
        {
            using var scope = Services.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            var waterService = scope.ServiceProvider.GetRequiredService<IWaterService>();
            var watersDtos = await waterService.SelectAllWatersAsync();
            if (watersDtos == null)
                return;
            var waters = new HashSet<Water>();
            foreach (var waterDto in watersDtos)
                waters.Add(new Water(waterDto));
            cache.Set(Cache.Waters, waters);
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