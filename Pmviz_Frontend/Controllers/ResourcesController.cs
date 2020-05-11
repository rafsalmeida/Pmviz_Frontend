﻿using System;
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
    public class ResourcesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        // get the list of processes
                        var logList = JsonConvert.DeserializeObject<List<Log>>(apiResponse);
                        ViewData["processes"] = logList;
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorProcess = "Error retrieving processes. Please try again later";
                        return View();
                    }
                }
            }
        }

        public async Task<IActionResult> Process([FromQuery(Name = "id")] string processId)
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
                        return View("Process", "Resources");
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = "There are no statistics in this specific activity.";
                            return View("Process", "Resources");

                        }
                        ViewBag.ErrorActivity = "Error retrieving activities. Please try again later";
                        return View("Process", "Resources");
                    }
                }
            }


        }

        [HttpPost]
        public async Task<IActionResult> Process([FromQuery(Name = "id")] string processId, string activity)
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
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = "There are no statistics in this specific activity.";

                        }
                        ViewBag.ErrorActivity = "Error retrieving activities. Please try again later";
                    }
                }

                using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/processes/" + processId +"?activity=" + activity + "&threshold=0"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var data = JObject.Parse(apiResponse);

                        ViewData["activity"] = activity;
                        ViewData["parts"] = data["parts"];

                        var moulds = JArray.Parse(data["moulds"].ToString());
                        if (moulds.Count == 0)
                        {
                            ViewData["moulds"] = "";
                        }
                        else
                        {
                            ViewData["moulds"] = moulds;
                        }

                        var resources = JArray.Parse(data["resources"].ToString());
                        if (resources.Count == 0)
                        {
                            ViewBag.ErrorActivity = "There are no records of any work in this activity.";
                            return View("Process", "Resources");
                        }


                        var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["meanMillis"].ToString()));
                        ViewData["meanMillis"] = timeSpan.TotalMinutes;

                        var allResources = JsonConvert.DeserializeObject<List<ResourceStat>>(data["resources"].ToString());
                        foreach (var r in allResources)
                        {
                            var timeSpanR = TimeSpan.FromMilliseconds(r.MeanMillis);

                            r.MeanMillis = timeSpanR.TotalMinutes;
                        }

                        allResources = allResources.OrderByDescending(x => x.MeanMillis).ThenBy(x => x.Resource).ToList();

                        ViewData["allResources"] = allResources;



                        return View("Process", "Resources");
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = "There are no records of any work in this activity.";
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