using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pmviz_Frontend.Models;

namespace Pmviz_Frontend.Controllers
{
    public class ActivityController : Controller
    {
        public async Task<IActionResult> Index([FromQuery(Name = "id")] string logid)
        {

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/activity-frequency/"+logid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true) {
                        var activityList = JsonConvert.DeserializeObject<List<ActivityFreq>>(apiResponse);

                        return View(activityList);
                    }
                    else
                    {
                        //handle
                    }
                }
            }
            return View();
        }
    }
}