using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pmviz_Frontend.Models;

namespace Pmviz_Frontend.Controllers
{
    public class DiagramController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string apiResponse; 
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/activity-frequency/1"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    /*var r = JsonConvert.DeserializeObject<String>(apiResponse);*/
                }
            }
            return Content(apiResponse);
        }

        public async Task<IActionResult> _Event()
        {
            List<Event> eventList = new List<Event>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    /*eventList = JsonConvert.DeserializeObject<List<Event>>(apiResponse);*/
                }
            }
            return Json(eventList);
        }

        public IActionResult Back()
        {
            var obj = JObject.Parse(HttpContext.Session.GetString("userDetails"));
            var role = obj["role"];

            return RedirectToAction("Index", role.ToString());
        }
    }
}