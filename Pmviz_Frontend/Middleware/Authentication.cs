using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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
            if(httpContext.Request.Path == "/" || httpContext.Request.Path == "/login" || httpContext.Request.Path == "/register")
            {
                return _next(httpContext);
            } else
            {
                var hasToken = httpContext.Session.GetString("sessionKey") != null ? true : false;

                if (hasToken)
                {
                    var obj = JObject.Parse(httpContext.Session.GetString("userDetails"));
                    var role = obj["role"].ToString();


                    // AUTHORIZATION VIA XML
                    var isAuthorized = ReadXML(role, httpContext);
                    GetDetailsToHide(role, httpContext);
                    if (isAuthorized)
                    {
                        return _next(httpContext);

                    }


                }
            }
            return httpContext.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString());
        }

        public bool ReadXML(string role, HttpContext httpContext)
        {
            if (!File.Exists($"../Pmviz_Frontend/Files/{role}.xml"))
            {
                new XDocument(
                    new XElement("root", new XElement("path",""))
                )
                .Save($"../Pmviz_Frontend/Files/{role}.xml");

            }
            XmlDocument doc = new XmlDocument();
            doc.Load($"../Pmviz_Frontend/Files/{role}.xml");

            var paths = doc.DocumentElement.SelectNodes("/root/path");
            var isAuthorized = true;

            for (int i = 0; i < paths.Count; i++)
            {
                if((httpContext.Request.Path == paths[i].InnerText || httpContext.Request.Path.StartsWithSegments(paths[i].InnerText) && 
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

            System.Diagnostics.Debug.WriteLine("Estou a contar: "+listDetails.Count);

            //STORE THE DETAILS NAMES ON SESSION STORAGE
            string details = string.Join(",", listDetails);
            System.Diagnostics.Debug.WriteLine(details);
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
