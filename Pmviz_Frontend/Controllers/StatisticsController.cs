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
                            ViewBag.ErrorProcess = await response.Content.ReadAsStringAsync();
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
                        // get the list of activities
                        var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                        activityList.RemoveAt(6);
                        ViewData["activities"] = activityList;
                        ViewData["processId"] = processId;
                        return View("Process", "Statistics");
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();
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
                        activityList.RemoveAt(6);
                        ViewData["activities"] = activityList;
                        ViewData["processId"] = processId;
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                        }
                        ViewBag.ErrorActivity = "Error retrieving activities. Please try again later";
                    }
                }

                using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/" + processId + "/resource/" + username + "?activity=" + activity))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var data = JObject.Parse(apiResponse);

                        ViewData["activity"] = activity;
                        //Parse moulds

                        if (data["moulds"].ToString() == "" || data["moulds"] == null)
                        {
                            ViewData["moulds"] = null;
                        }
                        else
                        {
                            var moulds = JArray.Parse(data["moulds"].ToString());
                            if (moulds.Count == 0)
                            {
                                ViewData["moulds"] = null;
                            }
                            else
                            {
                                var allMoulds = moulds.ToObject<List<string>>();
                                ViewData["moulds"] = allMoulds;

                            }

                        }


                        // Parse parts
                        if (data["parts"].ToString() == "")
                        {
                            ViewData["parts"] = null;
                        }
                        else
                        {
                            var parts = JArray.Parse(data["parts"].ToString());
                            if (parts.Count == 0)
                            {
                                ViewData["parts"] = null;
                            }
                            else
                            {
                                var allParts = parts.ToObject<List<string>>();
                                ViewData["parts"] = allParts;

                            }
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
                        ViewData["meanMillis"] = timeSpan.TotalMinutes.ToString("N2");
                       
                        var timeSpanUser = TimeSpan.FromMilliseconds(Double.Parse(millisUser.ToString()));
                        ViewData["meanMillisUser"] = timeSpanUser.TotalMinutes.ToString("N2");


                        return View("Process", "Statistics");
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();
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