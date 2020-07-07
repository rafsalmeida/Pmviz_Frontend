using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var hasToken = httpContext.Session.GetString("sessionKey") != null ? true : false;
            if (httpContext.Request.Path == "/" || httpContext.Request.Path == "/login")
            {
                if (hasToken)
                {
                    var isExpired = IsExpired(httpContext);
                    if(isExpired)
                    {
                        httpContext.Session.Clear();
                        httpContext.Response.Redirect("/login");
                        return httpContext.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString());
                    }

                    httpContext.Response.Redirect("/home");
                    return httpContext.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString());
                }
                else
                {
                    return _next(httpContext);
                }
            }
            else
            {
                if (hasToken)
                {
                    var obj = JObject.Parse(httpContext.Session.GetString("userDetails"));
                    var role = obj["role"].ToString();

                    var isExpired = IsExpired(httpContext);
                    if (isExpired)
                    {
                        httpContext.Session.Clear();
                        httpContext.Response.Redirect("/login");
                        return httpContext.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString());
                    }

                    // AUTHORIZATION VIA XML
                    var isAuthorized = ReadXML(role, httpContext);
                    GetDetailsToHide(role, httpContext);
                    if (isAuthorized)
                    {
                        return _next(httpContext);

                    }


                }
            }
            if (hasToken)
            {
                httpContext.Response.Redirect("/home");
            }
            else
            {
                httpContext.Response.Redirect("/login");
            }

            return httpContext.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString());
        }

        public bool IsExpired(HttpContext httpContext)
        {
            var obj = JObject.Parse(httpContext.Session.GetString("userDetails"));
            var expDate = Convert.ToInt32(obj["exp"].ToString());
            Int32 timestampNow = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if(timestampNow >= expDate)
            {
                return true;
            }

            return false;
            
        }
        public bool ReadXML(string role, HttpContext httpContext)
        {
            if (!File.Exists($"../Pmviz_Frontend/Files/{role}.xml"))
            {
                new XDocument(
                    new XElement("root", new XElement("path", ""))
                )
                .Save($"../Pmviz_Frontend/Files/{role}.xml");

            }
            XmlDocument doc = new XmlDocument();
            doc.Load($"../Pmviz_Frontend/Files/{role}.xml");

            var paths = doc.DocumentElement.SelectNodes("/root/path");
            var isAuthorized = true;

            for (int i = 0; i < paths.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(paths[i].InnerText);
                if ((httpContext.Request.Path == paths[i].InnerText.Trim() || httpContext.Request.Path.StartsWithSegments(paths[i].InnerText.Trim()) &&
                    paths[i].InnerText != ""))
                {
                    isAuthorized = false;
                }
            }
            return isAuthorized;
        }

        public void GetDetailsToHide(string role, HttpContext httpContext)
        {
            var authorizationController = new Pmviz_Frontend.Controllers.AuthorizationController();

            //GET THE LIST OF DETAILS NOT ALLOWED TO SEE
            var listDetails = authorizationController.GetDetailsNotAllowedToSee(role);

            //STORE THE DETAILS NAMES ON SESSION STORAGE
            string details = string.Join(",", listDetails);
            httpContext.Session.SetString("listDetails", details);
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
