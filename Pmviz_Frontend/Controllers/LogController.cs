using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pmviz_Frontend.Models;

namespace PmvizFrontend.Controllers
{
    public class LogController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Log> logList = new List<Log>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/logs"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if(status == true)
                    {
                        logList = JsonConvert.DeserializeObject<List<Log>>(apiResponse);
                        return View(logList);
                    } else
                    {
                        return RedirectToAction("Index", "Home", new { error = "1"});
                    }
                }
            }
        }

        public async Task<IActionResult> Activities([FromQuery(Name = "id")] string logid)
        {
           
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/logs/"+logid+"/activities"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        System.Diagnostics.Debug.WriteLine(apiResponse);
                        var activities = JsonConvert.DeserializeObject<string[]>(apiResponse);
                        var activitiesList = new List<Activity>();
                        foreach (var a in activities)
                        {
                            Activity act = new Activity
                            {
                                Name = a
                            };
                            activitiesList.Add(act);

                        }
                        return View("Activities",activitiesList);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                }
            }
        }

        public async Task<IActionResult> Resources([FromQuery(Name = "id")] string logid)
        {

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/logs/" + logid + "/resources"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        System.Diagnostics.Debug.WriteLine(apiResponse);
                        var resources = JsonConvert.DeserializeObject<string[]>(apiResponse);
                        var resourcesList = new List<Resource>();
                        foreach (var r in resources)
                        {
                            Resource res = new Resource
                            {
                                Name = r
                            };
                            resourcesList.Add(res);

                        }
                        return View("Resources", resourcesList);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                }
            }
        }

        public IActionResult Back()
        {
            var obj = JObject.Parse(HttpContext.Session.GetString("userDetails"));
            var role = obj["role"];

            return RedirectToAction("Index", role.ToString());
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
