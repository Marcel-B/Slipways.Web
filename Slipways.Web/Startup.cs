using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using com.b_velop.Slipways.Web.Infrastructure;
using GraphQL.Client;
using com.b_velop.Slipways.Web.Services;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.IdentityProvider;
using com.b_velop.IdentityProvider.Model;
using Microsoft.AspNetCore.HttpOverrides;
using Prometheus;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var user = Environment.GetEnvironmentVariable("SEND_GRID_USER");
            var graphQLEndpoint = Environment.GetEnvironmentVariable("GRAPH_QL_ENDPOINT");
            var authority = Environment.GetEnvironmentVariable("AUTHORITY");
            var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
            var scope = Environment.GetEnvironmentVariable("SCOPE");


            var secretProvider = new SecretProvider();
            var clientSecret = secretProvider.GetSecret("slipways.web");
            var key = secretProvider.GetSecret("send_grid_key");
            if (Env.IsDevelopment())
            {
                key = Environment.GetEnvironmentVariable("SEND_GRID_KEY");
                clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            }


            services.AddSingleton(_ => new InfoItem(clientId, clientSecret, scope, authority));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient(_ => new AuthMessageSenderOptions
            {
                SendGridKey = key,
                SendGridUser = user
            });
            services.AddScoped(_ => new GraphQLClient(graphQLEndpoint));
            services.AddHttpClient<ISlipwayService, SlipwayService>();
            services.AddHttpClient<IIdentityProviderService, IdentityProviderService>();

            services.AddDataProtection()
               .SetApplicationName("slipways-web")
               .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/app/keys/"));

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Slipways");
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }
}
