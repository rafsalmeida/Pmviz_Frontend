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
                        string roleToDelete ="";
                        foreach(var r in roles){
                            if(r.Trim() == "Administrator")
                            {
                                roleToDelete = r;
                            }
                        }
                        roles.Remove(roleToDelete);
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

            var hasLog = false;
            var hasActivity = false;
            var hasresource = false;

            foreach (var route in newRoutesNotAllowed)
            {
                if(route.Trim() == "/log")
                {
                    hasLog = true;
                }

                if(route.Trim() == "/log/resource")
                {
                    hasresource = true;

                }

                if (route.Trim() == "/log/activity")
                {
                     hasActivity = true;

                }

            }
           
            if(hasLog)
            {
                if (!hasresource)
                {
                    newRoutesNotAllowed.Add("/log/resource");
                }

                if (!hasActivity)
                {
                    newRoutesNotAllowed.Add("/log/activity");
                }
            }


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
                        string roleToDelete = "";
                        foreach (var r in roles)
                        {
                            if (r.Trim() == "Administrator")
                            {
                                roleToDelete = r;
                            }
                        }
                        roles.Remove(roleToDelete);
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
                    
                    if (allPaths[i].InnerText.Trim() == paths[j].InnerText.Trim())
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

            var btns = docAssociation.SelectNodes("//button");
                
            // REMOVE TEMPORARILY THE BUTTONS BECAUSE THEY APPEAR ON THE INNER TEXT OF THE PARENT
            for (int i = 0; i < btns.Count; i++)
            {
                btns[i].ParentNode.RemoveChild(btns[i]);
            }

            var paths = docAssociation.DocumentElement.SelectNodes("/root/path");



            return paths;
        }

        public XmlNodeList ReadXMLAssociationWithDetails()
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

        public List<String> GetDetailsNotAllowedToSee(string role)
        {
            var routesNotAllowed = RoutesNotAllowed(role);
            var paths = ReadXMLAssociationWithDetails();
            var listDetails = new List<String>();


            for (int i = 0; i < paths.Count; i++)
            {

                // SEE IF THAT ROLE CANNOT SEE THE PATH
                if (routesNotAllowed.Contains(paths[i].Attributes["name"].Value))
                {

                    XmlNodeList buttons = paths[i].ChildNodes;
                    for (int j = 0; j < buttons.Count; j++)
                    { 
                        // GET THE CHILD NODES THAT CONTAIN THE BUTTONS/DETAILS THE ROLE CANNOT SEE
                        if(buttons[j].GetType() == typeof(System.Xml.XmlElement))
                        {
                            listDetails.Add(buttons[j].InnerText);
                        }
                    }

                }
            }

            return listDetails;
            

        }

    }
}