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

namespace Pmviz_Frontend.Controllers
{
    public class StatisticsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var obj = JObject.Parse(HttpContext.Session.GetString("userDetails"));
            var username = obj["sub"];

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/users/"+username+"/processes"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        // get the list of logs
                        var logList = JsonConvert.DeserializeObject<List<Log>>(apiResponse);
                        ViewData["processes"] = logList;
                        return View();
                    }
                    else
                    {
                        if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorProcess = "You haven't done any activity in any process.";
                            return View();

                        }
                        ViewBag.ErrorProcess = "Error retrieving processes. Please try again later";
                        return View();
                    }
                }
            }
        }

        public async Task<IActionResult> Process([FromQuery(Name ="id")] string processId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        // get the list of logs
                        var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                        ViewData["activities"] = activityList;
                        ViewData["processId"] = processId;
                        return View("Process", "Statistics");
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = "You haven't done this specific activity in any process.";
                            return View("Process", "Statistics");

                        }
                        ViewBag.ErrorActivity = "Error retrieving activities. Please try again later";
                        return View("Process", "Statistics");
                    }
                }
            }


        }

        [HttpPost]
        public async Task<IActionResult> Process([FromQuery(Name = "id")] string processId, string activity)
        {
            var obj = JObject.Parse(HttpContext.Session.GetString("userDetails"));
            var username = obj["sub"];

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        // get the list of logs
                        var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                        ViewData["activities"] = activityList;
                        ViewData["processId"] = processId;
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = "You haven't done this specific activity in any process.";

                        }
                        ViewBag.ErrorActivity = "Error retrieving activities. Please try again later";
                    }
                }

                using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/processes/" + processId + "/resource/" + username + "?activity=" + activity))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var data = JObject.Parse(apiResponse);

                        ViewData["activity"] = activity;
                        ViewData["parts"] = data["parts"];

                        var moulds = JArray.Parse(data["moulds"].ToString());
                        if(moulds.Count == 0)
                        {
                            ViewData["moulds"] = "";
                        } else
                        {
                            ViewData["moulds"] = moulds;
                        }

                        var resource = JArray.Parse(data["resources"].ToString());
                        if(resource.Count == 0)
                        {
                            ViewBag.ErrorActivity = "There are no records of your mean times in this activity.";
                            return View("Process", "Statistics");
                        }

                        var objj = resource[0];
                        var millisUser = objj["meanMillis"];

                        var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["meanMillis"].ToString()));
                        ViewData["meanMillis"] = timeSpan.TotalMinutes;


                        var timeSpanUser = TimeSpan.FromMilliseconds(Double.Parse(millisUser.ToString()));
                        ViewData["meanMillisUser"] = timeSpanUser.TotalMinutes;


                        return View("Process", "Statistics");
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = "You haven't done this specific activity in any process.";
                            return View("Process", "Statistics");

                        }
                        ViewBag.ErrorActivity = "Error retrieving activities. Please try again later";
                        return View("Process", "Statistics");

                    }
                }

            }


        }


    }
}