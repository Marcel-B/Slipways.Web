using System;
using com.b_velop.IdentityProvider;
using com.b_velop.IdentityProvider.Model;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using com.b_velop.Slipways.Web.ViewModels;
using GraphQL.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
namespace com.b_velop.Slipways.Web
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Env = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            var cache = Environment.GetEnvironmentVariable("CACHE");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cache;
                options.InstanceName = "Slipways";
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            var sendGridUser = Environment.GetEnvironmentVariable("SEND_GRID_USER");
            var graphQLEndpoint = Environment.GetEnvironmentVariable("GRAPH_QL_ENDPOINT");
            var authority = Environment.GetEnvironmentVariable("AUTHORITY");
            var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
            var scope = Environment.GetEnvironmentVariable("SCOPE");
            var secretProvider = new SecretProvider();
            var clientSecret = secretProvider.GetSecret("slipways_web");
            var key = secretProvider.GetSecret("send_grid_key");
            var appId = secretProvider.GetSecret("facebook_app_id");
            var appSecret = secretProvider.GetSecret("facebook_app_secret");
            if (Env.IsDevelopment())
            {
                key = Environment.GetEnvironmentVariable("SEND_GRID_KEY");
                clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                appId = Environment.GetEnvironmentVariable("facebook_app_id");
                appSecret = Environment.GetEnvironmentVariable("facebook_app_secret");
            }
            services.AddSingleton(_ => new InfoItem(clientId, clientSecret, scope, authority));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient(_ => new AuthMessageSenderOptions
            {
                SendGridKey = key,
                SendGridUser = sendGridUser
            });
            services.AddScoped(_ => new GraphQLClient(graphQLEndpoint));
            services.AddScoped<ISecretProvider, SecretProvider>();
            services.AddScoped<IStoreWrapper, StoreWrapper>();
            services.AddScoped<ISlipwayStore, SlipwayStore>();
            services.AddScoped<IExtraStore, ExtraStore>();
            services.AddScoped<IManufacturerStore, ManufacturerStore>();
            services.AddScoped<IWaterStore, WaterStore>();
            services.AddScoped<IServiceStore, ServiceStore>();
            services.AddScoped<WaterViewModel>();
            services.AddScoped<IGraphQLService, GraphQLService>();

            services.AddHttpClient<ISlipwayService, SlipwayService>("slipwayClient", options =>
            {
                options.BaseAddress = new Uri("http://slipways-api:80/api/slipways");
                if (Env.IsProduction())
                {
                    options.BaseAddress = new Uri("http://slipways-api/api/slipways");
                }
            });

            services.AddHttpClient<IServiceService, ServiceService>("serviceClient", options =>
            {
                options.BaseAddress = new Uri("http://slipways-api:80/api/service");
                if (Env.IsProduction())
                {
                    options.BaseAddress = new Uri("http://slipways-api/api/service");
                }
            });
            services.AddHttpClient<IIdentityProviderService, IdentityProviderService>();
            services.AddHttpClient<IExtraService, ExtraService>("extraClient", options =>
            {
                options.BaseAddress = new Uri("http://slipways-api/api/extra");
            });
            services.AddHttpClient<IWaterService, WaterService>("waterClient", options =>
            {
                options.BaseAddress = new Uri("http://slipways-api/api/water");
            });
            services.AddHttpClient<IManufacturerService, ManufacturerService>("manufacturerClient", options =>
            {
                options.BaseAddress = new Uri("http://slipways-api/api/manufacturer");
            });

            if (!Env.IsDevelopment())
                services.AddDataProtection()
                .SetApplicationName("slipways-web")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/app/keys/"));

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Slipways");
                    if (!Env.IsDevelopment())
                    {
                        options.Conventions.AuthorizeAreaFolder("Admin", "/Slipway");
                        options.Conventions.AuthorizeAreaFolder("Admin", "/Water");
                        options.Conventions.AuthorizeAreaFolder("Admin", "/Service");
                    }
                });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var pw = string.Empty;

            if (Env.IsProduction())
                pw = secretProvider.GetSecret("sqlserver");
            else if (Env.IsStaging())
                pw = secretProvider.GetSecret("dev_slipway_db");
            else
                pw = "foo123bar!";

            var server = Environment.GetEnvironmentVariable("SERVER");
            var user = Environment.GetEnvironmentVariable("USER");
            var db = Environment.GetEnvironmentVariable("DATABASE");
            var port = Environment.GetEnvironmentVariable("PORT");

            var str = $"Server={server},{port};Database={db};User Id={user};Password={pw}";

            services.AddDbContext<ApplicationDbContext>(_ => _.UseSqlServer(str));
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            app.UseHttpMetrics(options =>
            {
                options.RequestCount.Enabled = false;
                options.RequestDuration.Histogram = Metrics.CreateHistogram("slipways_web_http_request_duration_seconds", "Some help text",
                    new HistogramConfiguration
                    {
                        Buckets = Histogram.LinearBuckets(start: 1, width: 1, count: 64),
                        LabelNames = new[] { "code", "method" }
                    });
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }
            UpdateDatabase(app);
            app.UseCookiePolicy();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapMetrics();
            });
        }

        private static void UpdateDatabase(
            IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }
}
