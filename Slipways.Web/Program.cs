using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Prometheus;
using System;

namespace com.b_velop.Slipways.Web
{
    public class Program
    {
        public static string Env = "";
        public static void Main(string[] args)
        {
            var pusher = new MetricPusher(new MetricPusherOptions
            {
                Endpoint = "https://push.qaybe.de/metrics",
                Job = "SlipwaysWeb",
                Instance = "SlipwaysWeb"
            });

            pusher.Start();

            var file = "dev-nlog.config";
            Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (Env == "Production")
                file = "nlog.config";

            var logger = NLogBuilder.ConfigureNLog(file).GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .ConfigureLogging(logging =>
              {
                  logging.ClearProviders();
                  logging.AddConsole();
                  logging.SetMinimumLevel(LogLevel.Trace);
              })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices((hostingContet, services) =>
                {
                    var secretProvider = new SecretProvider();
                    var pw = string.Empty;

                    if (Env != "Production")
                        pw = secretProvider.GetSecret("dev_slipway_db");
                    else if (Env == "Production")
                        pw = secretProvider.GetSecret("sqlserver");
                    else
                        pw = "foo123bar!";

                    var server = Environment.GetEnvironmentVariable("SERVER");
                    var user = Environment.GetEnvironmentVariable("USER");
                    var db = Environment.GetEnvironmentVariable("DATABASE");
                    var port = Environment.GetEnvironmentVariable("PORT");




                    var str = $"Server={server},{port};Database={db};User Id={user};Password={pw}";
#if DEBUG
                    str = "Server=localhost,1433;Database=Slipways;User Id=sa;Password=foo123bar!";
#endif
                    services.AddDbContext<ApplicationDbContext>(_ => _.UseSqlServer(str));
                })
                .UseNLog();
    }
}
