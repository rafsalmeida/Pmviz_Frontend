using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace Pmviz_Frontend.Controllers
{
    public class AuthorizationController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/users/roles"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var roles = JsonConvert.DeserializeObject<List<String>>(apiResponse);
                        ViewData["roles"] = roles;
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Roles not available. Please try again later.";
                        return View();
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult Save(string roleChosen, List<string> allowed, List<string> notAllowed)
        {
            var newRoutesNotAllowed = GetRouteFromNames(notAllowed);

            UpdateXmlFile(roleChosen, newRoutesNotAllowed);

           // return View();
            return RedirectToAction("Index","Home", new { @success = "1"});
        }

        [HttpPost]
        public async Task<IActionResult> Index(string role)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/users/roles"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var roles = JsonConvert.DeserializeObject<List<String>>(apiResponse);
                        ViewData["roles"] = roles;
                    }
                    else
                    {
                        ViewData["roles"] = new List<string>();
                    }
                }
            }

            var allRoutes = GetAllRoutes();
            var routesNotAllowed = RoutesNotAllowed(role);
            var routesAllowed = RoutesAllowed(allRoutes, routesNotAllowed);
            ViewData["RoutesAllowed"] = routesAllowed;
            ViewData["RoutesNotAllowed"] = routesNotAllowed;
            ViewData["Role"] = role;
            return View();

        }

        public List<String> RoutesNotAllowed(string role)
        {
            //READ BOTH FILES
            var allPaths = ReadXMLAssociation();
            var doc = ReadRoleXmlFile(role);


            var paths = doc.DocumentElement.SelectNodes("/root/path");
            var routesNameNotAllowed = new List<string>();

            for (int i = 0; i < allPaths.Count; i++)
            {
                for (int j = 0; j < paths.Count; j++)
                {
                    System.Diagnostics.Debug.WriteLine("Path i : " + allPaths[i].InnerText + " path 2: " + paths[j].InnerText);

                    if (allPaths[i].InnerText == paths[j].InnerText)
                    {
                        routesNameNotAllowed.Add(allPaths[i].Attributes["name"].Value);
                    }
                }
            }
            return routesNameNotAllowed;
        }


        public List<String> RoutesAllowed(List<string> allRoutes, List<string> routesNotAllowed)
        {
            var routesAllowed = allRoutes;
            foreach(var route in routesNotAllowed)
            {
                if (allRoutes.Contains(route))
                {
                    routesAllowed.Remove(route);
                }
            }

            return routesAllowed;

        }

        public XmlNodeList ReadXMLAssociation()
        {
            // READ XML ASSOCIATION FILE
            XmlDocument docAssociation = new XmlDocument();
            docAssociation.Load("../Pmviz_Frontend/Files/Association.xml");

            var paths = docAssociation.DocumentElement.SelectNodes("/root/path");

            return paths;
        }

        public List<string> GetAllRoutes()
        {
            // GET ALL ROUTE NAMES FROM ASSOCIATION FILE
            var paths = ReadXMLAssociation();
            var names = new List<string>();
            for (int i = 0; i < paths.Count; i++)
            {
                names.Add(paths[i].Attributes["name"].Value);
            }

            return names;
        }

        public XmlDocument ReadRoleXmlFile(string role)
        {
            if (!System.IO.File.Exists($"../Pmviz_Frontend/Files/{role}.xml"))
            {
                new XDocument(
                    new XElement("root", new XElement("path", ""))
                )
                .Save($"../Pmviz_Frontend/Files/{role}.xml");

            }

            XmlDocument doc = new XmlDocument();
            doc.Load($"../Pmviz_Frontend/Files/{role}.xml");

            return doc;

        }

        public List<string> GetRouteFromNames(List<string> notAllowed)
        {
            var paths = ReadXMLAssociation();
            var newNotAllowed = new List<string>();

            for (int i = 0; i < paths.Count; i++)
            {
                foreach (var route in notAllowed)
                {
                    if (paths[i].Attributes["name"].Value == route)
                    {
                        newNotAllowed.Add(paths[i].InnerText);
                    }
                }
            }

            return newNotAllowed;

        }

        public void UpdateXmlFile(string role, List<string> routes)
        {
            // DELETE FILE AND CREATE A NEW FILE
            System.IO.File.Delete($"../Pmviz_Frontend/Files/{role}.xml");

            new XDocument(
                   new XElement("root", new XElement("path", ""))
               )
               .Save($"../Pmviz_Frontend/Files/{role}.xml");

            // LOAD THE FILE

            XmlDocument doc = new XmlDocument();
            doc.Load($"../Pmviz_Frontend/Files/{role}.xml");

            // FILL THE FILE WITH THE NEW INFORMATION

            XmlElement root = (XmlElement) doc.DocumentElement.SelectSingleNode("/root");
            foreach(var route in routes)
            {
                XmlElement element = doc.CreateElement("path");
                element.InnerText = route;
                root.AppendChild(element);
            }

            doc.Save($"../Pmviz_Frontend/Files/{role}.xml");

        }

    }
}