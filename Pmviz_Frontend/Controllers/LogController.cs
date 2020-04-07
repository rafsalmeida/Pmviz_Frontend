using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PmvizFrontend.Models;

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
                    logList = JsonConvert.DeserializeObject<List<Log>>(apiResponse);
                }
            }
            return View(logList);
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
