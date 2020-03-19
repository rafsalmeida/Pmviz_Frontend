using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
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
