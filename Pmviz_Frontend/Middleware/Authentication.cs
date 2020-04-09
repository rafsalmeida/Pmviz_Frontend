using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Pmviz_Frontend.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Authentication
    {
        private readonly RequestDelegate _next;

        public Authentication(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if(httpContext.Request.Path == "/" || httpContext.Request.Path == "/login" || httpContext.Request.Path == "/login/logout")
            {
                return _next(httpContext);
            } else
            {
                var hasToken = httpContext.Session.GetString("sessionKey") != null ? true : false;

                if (hasToken)
                {
                    var obj = JObject.Parse(httpContext.Session.GetString("userDetails"));
                    var role = obj["role"];
                    return _next(httpContext);

                    /*if (httpContext.Request.Path == "" || httpContext.Request.Path.ToString().Contains(role.ToString()))
                    {
                        return _next(httpContext);
                    }*/
                }
            }
            return httpContext.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString());
        }


    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationExtensions
    {
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Authentication>();
        }


    }
}
