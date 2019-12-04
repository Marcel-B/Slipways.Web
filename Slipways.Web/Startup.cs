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

namespace com.b_velop.Slipways.Web
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var key = Environment.GetEnvironmentVariable("SEND_GRID_KEY");
            var user = Environment.GetEnvironmentVariable("SEND_GRID_USER");
            var graphQLEndpoint = Environment.GetEnvironmentVariable("GRAPH_QL_ENDPOINT");
            var authority = Environment.GetEnvironmentVariable("AUTHORITY");
            var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            var scope = Environment.GetEnvironmentVariable("SCOPE");

            services.AddSingleton<InfoItem>(_ => new InfoItem(clientId, clientSecret, scope, authority));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient(_ => new AuthMessageSenderOptions
            {
                SendGridKey = key,
                SendGridUser = user
            });
            services.AddScoped(_ => new GraphQLClient(graphQLEndpoint));
            services.AddHttpClient<ISlipwayService, SlipwayService>();
            services.AddHttpClient<IIdentityProviderService, IdentityProviderService>();

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Slipways");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            UpdateDatabase(app);
            app.UseHttpsRedirection();
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
