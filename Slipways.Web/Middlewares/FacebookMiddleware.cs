using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace com.b_velop.Slipways.Web.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class FacebookMiddleware
    {
        private readonly RequestDelegate _next;

        public FacebookMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(
            HttpContext httpContext)
        {
            if (httpContext.Request.Host.Value.Contains("facebook"))
                httpContext.Request.Scheme = "https";
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FacebookMiddlewareExtensions
    {
        public static IApplicationBuilder UseFacebookMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FacebookMiddleware>();
        }
    }
}
