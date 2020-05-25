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
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if(status == true)
                    {
                        // get the list of logs
                        logList = JsonConvert.DeserializeObject<List<Log>>(apiResponse);
                        return View(logList);
                    } else
                    {
                        return RedirectToAction("Index", "Home", new { error = "1"});
                    }
                }
            }
        }

        public IActionResult Activity([FromQuery(Name = "id")]string processId)
        {
            ViewData["processId"] = processId;
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Activity([FromQuery(Name = "id")]string processId, string type)
        {
            ViewData["processId"] = processId;

            if (type == "frequency")
            {
                #region Frequency 
                ViewData["type"] = "frequency";
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:8080/api/activity-frequency/" + processId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var status = response.IsSuccessStatusCode;
                        if (status == true)
                        {
                            var activityList = JsonConvert.DeserializeObject<List<ActivityFreq>>(apiResponse);
                            foreach (var a in activityList)
                            {
                                var meanTime = a.MeanDuration.Days != 0 ? a.MeanDuration.Days + "d " : "";
                                meanTime += a.MeanDuration.Hours != 0 ? a.MeanDuration.Hours + "h " : "";
                                meanTime += a.MeanDuration.Minutes != 0 ? a.MeanDuration.Minutes + "m " : "";
                                meanTime += a.MeanDuration.Seconds != 0 ? a.MeanDuration.Seconds + "s " : "";
                                meanTime += a.MeanDuration.Millis != 0 ? a.MeanDuration.Millis + "ms " : " 0ms";
                                a.MeanActivityFormatted = meanTime;

                                var tsMean = new TimeSpan(a.MeanDuration.Days, a.MeanDuration.Hours, a.MeanDuration.Minutes, a.MeanDuration.Seconds, a.MeanDuration.Millis);

                                a.MeanInMinutes = tsMean.TotalMinutes;

                                var medianTime = a.MedianDuration.Days != 0 ? a.MedianDuration.Days + "d " : "";
                                medianTime += a.MedianDuration.Hours != 0 ? a.MedianDuration.Hours + "h " : "";
                                medianTime += a.MedianDuration.Minutes != 0 ? a.MedianDuration.Minutes + "m " : "";
                                medianTime += a.MedianDuration.Seconds != 0 ? a.MedianDuration.Seconds + "s " : "";
                                medianTime += a.MedianDuration.Millis != 0 ? a.MedianDuration.Millis + "ms " : " 0ms";
                                a.MedianActivityFormatted = medianTime;

                                var tsMedian = new TimeSpan(a.MedianDuration.Days, a.MedianDuration.Hours, a.MedianDuration.Minutes, a.MedianDuration.Seconds, a.MedianDuration.Millis);

                                a.MedianInMinutes = tsMedian.TotalMinutes;

                                var minTime = a.MinDuration.Days != 0 ? a.MinDuration.Days + "d " : "";
                                minTime += a.MinDuration.Hours != 0 ? a.MinDuration.Hours + "h " : "";
                                minTime += a.MinDuration.Minutes != 0 ? a.MinDuration.Minutes + "m " : "";
                                minTime += a.MinDuration.Seconds != 0 ? a.MinDuration.Seconds + "s " : "";
                                minTime += a.MinDuration.Millis != 0 ? a.MinDuration.Millis + "ms " : " 0ms";
                                a.MinActivityFormatted = minTime;

                                var maxTime = a.MaxDuration.Days != 0 ? a.MaxDuration.Days + "d " : "";
                                maxTime += a.MaxDuration.Hours != 0 ? a.MaxDuration.Hours + "h " : "";
                                maxTime += a.MaxDuration.Minutes != 0 ? a.MaxDuration.Minutes + "m " : "";
                                maxTime += a.MaxDuration.Seconds != 0 ? a.MaxDuration.Seconds + "s " : "";
                                maxTime += a.MaxDuration.Millis != 0 ? a.MaxDuration.Millis + "ms " : " 0ms";
                                a.MaxActivityFormatted = maxTime;


                            }

                            IEnumerable<ActivityFreq> al = activityList.OrderByDescending(x => x.Frequency).ThenBy(x => x.Activity).ToList();
                            ViewData["Frequency"] = al;
                            al = activityList.OrderByDescending(x => x.MeanInMinutes).ThenBy(x => x.Activity).ToList();
                            ViewData["Mean"] = al;
                            al = activityList.OrderByDescending(x => x.MedianInMinutes).ThenBy(x => x.Activity).ToList();
                            ViewData["Median"] = al;

                            return View(activityList);
                        }
                        else
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                            {
                                ViewBag.Error = await response.Content.ReadAsStringAsync();
                                return View();

                            }
                            ViewBag.Error = "Error retrieving statistics. Please, try again later.";
                            return View();

                        }
                    }
                }
                #endregion

            }
            else if(type == "effort")
            {
                #region Effort 
                ViewData["type"] = "effort";
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:8080/api/activity-frequency/effort/" + processId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var status = response.IsSuccessStatusCode;
                        if (status == true)
                        {
                            var activityList = JsonConvert.DeserializeObject<List<ActivityFreq>>(apiResponse);
                            foreach (var a in activityList)
                            {
                                var meanTime = a.MeanDuration.Days != 0 ? a.MeanDuration.Days + "d " : "";
                                meanTime += a.MeanDuration.Hours != 0 ? a.MeanDuration.Hours + "h " : "";
                                meanTime += a.MeanDuration.Minutes != 0 ? a.MeanDuration.Minutes + "m " : "";
                                meanTime += a.MeanDuration.Seconds != 0 ? a.MeanDuration.Seconds + "s " : "";
                                meanTime += a.MeanDuration.Millis != 0 ? a.MeanDuration.Millis + "ms " : " 0ms";
                                a.MeanActivityFormatted = meanTime;

                                var tsMean = new TimeSpan(a.MeanDuration.Days, a.MeanDuration.Hours, a.MeanDuration.Minutes, a.MeanDuration.Seconds, a.MeanDuration.Millis);

                                a.MeanInMinutes = tsMean.TotalMinutes;

                                var medianTime = a.MedianDuration.Days != 0 ? a.MedianDuration.Days + "d " : "";
                                medianTime += a.MedianDuration.Hours != 0 ? a.MedianDuration.Hours + "h " : "";
                                medianTime += a.MedianDuration.Minutes != 0 ? a.MedianDuration.Minutes + "m " : "";
                                medianTime += a.MedianDuration.Seconds != 0 ? a.MedianDuration.Seconds + "s " : "";
                                medianTime += a.MedianDuration.Millis != 0 ? a.MedianDuration.Millis + "ms " : " 0ms";
                                a.MedianActivityFormatted = medianTime;

                                var tsMedian = new TimeSpan(a.MedianDuration.Days, a.MedianDuration.Hours, a.MedianDuration.Minutes, a.MedianDuration.Seconds, a.MedianDuration.Millis);

                                a.MedianInMinutes = tsMedian.TotalMinutes;

                                var minTime = a.MinDuration.Days != 0 ? a.MinDuration.Days + "d " : "";
                                minTime += a.MinDuration.Hours != 0 ? a.MinDuration.Hours + "h " : "";
                                minTime += a.MinDuration.Minutes != 0 ? a.MinDuration.Minutes + "m " : "";
                                minTime += a.MinDuration.Seconds != 0 ? a.MinDuration.Seconds + "s " : "";
                                minTime += a.MinDuration.Millis != 0 ? a.MinDuration.Millis + "ms " : " 0ms";
                                a.MinActivityFormatted = minTime;

                                var maxTime = a.MaxDuration.Days != 0 ? a.MaxDuration.Days + "d " : "";
                                maxTime += a.MaxDuration.Hours != 0 ? a.MaxDuration.Hours + "h " : "";
                                maxTime += a.MaxDuration.Minutes != 0 ? a.MaxDuration.Minutes + "m " : "";
                                maxTime += a.MaxDuration.Seconds != 0 ? a.MaxDuration.Seconds + "s " : "";
                                maxTime += a.MaxDuration.Millis != 0 ? a.MaxDuration.Millis + "ms " : " 0ms";
                                a.MaxActivityFormatted = maxTime;


                            }

                            IEnumerable<ActivityFreq> al = activityList.OrderByDescending(x => x.Frequency).ThenBy(x => x.Activity).ToList();
                            ViewData["Frequency"] = al;
                            al = activityList.OrderByDescending(x => x.MeanInMinutes).ThenBy(x => x.Activity).ToList();
                            ViewData["Mean"] = al;
                            al = activityList.OrderByDescending(x => x.MedianInMinutes).ThenBy(x => x.Activity).ToList();
                            ViewData["Median"] = al;

                            return View(activityList);
                        }
                        else
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                            {
                                ViewBag.Error = await response.Content.ReadAsStringAsync();
                                return View();

                            }
                            ViewBag.Error = "Error retrieving statistics. Please, try again later.";
                            return View();

                        }
                    }
                }
                #endregion
            }
            else
            {
                ViewBag.Error = "Select an option!";
                return View();

            }

           

        }

        public async Task<IActionResult> Resource([FromQuery(Name = "id")]string processId)
        {
            ViewData["processId"] = processId;
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
                        return View();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();
                            return View();

                        }
                        ViewBag.ErrorActivity = "Error retrieving activities. Please try again later";
                        return View();
                    }
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> Resource([FromQuery(Name = "id")]string processId, string type, string activity)
        {
            ViewData["processId"] = processId;
            if(type == "mean")
            {
                #region Mean
                ViewData["type"] = "mean";

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

                    using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/" + processId + "?activity=" + activity))
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

                            var resources = JArray.Parse(data["resources"].ToString());
                            if (resources.Count == 0)
                            {
                                ViewBag.ErrorActivity = "There are no records of any work in this activity.";
                                return View();
                            }


                            var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["meanMillis"].ToString()));
                            ViewData["meanMillis"] = timeSpan.TotalMinutes;

                            var allResources = JsonConvert.DeserializeObject<List<ResourceStat>>(data["resources"].ToString());
                            foreach (var r in allResources)
                            {
                                var timeSpanR = TimeSpan.FromMilliseconds(r.MeanMillis);

                                r.MeanMillis = timeSpanR.TotalMinutes;
                                r.GeneralMean = timeSpan.TotalMinutes;
                            }

                            allResources = allResources.OrderByDescending(x => x.MeanMillis).ThenBy(x => x.Resource).ToList();

                            ViewData["allResources"] = allResources;



                            return View();
                        }
                        else
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();
                                return View();

                            }
                            ViewBag.ErrorActivity = "Error retrieving statistics. Please try again later";
                            return View();

                        }
                    }

                }


                #endregion

            }
            else if (type == "effort")
            {
                #region Effort
                ViewData["type"] = "effort";

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

                    using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/effort/" + processId + "?activity=" + activity))
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

                            var resources = JArray.Parse(data["resources"].ToString());
                            if (resources.Count == 0)
                            {
                                ViewBag.ErrorActivity = "There are no records of any work in this activity.";
                                return View();
                            }


                            var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["totalEffortMillis"].ToString()));
                            ViewData["totalEffortMillis"] = timeSpan.TotalMinutes;

                            var allResources = JsonConvert.DeserializeObject<List<ResourceEffort>>(data["resources"].ToString());
                            foreach (var r in allResources)
                            {
                                var timeSpanR = TimeSpan.FromMilliseconds(r.EffortMillis);

                                r.EffortMillis = timeSpanR.TotalMinutes;
                            }

                            allResources = allResources.OrderByDescending(x => x.EffortMillis).ThenBy(x => x.Resource).ToList();

                            ViewData["allResourcesEffort"] = allResources;



                            return View();
                        }
                        else
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();
                                return View();

                            }
                            ViewBag.ErrorActivity = "Error retrieving statistics. Please try again later";
                            return View();

                        }
                    }

                }


                #endregion


            }
            else
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

                }


                ViewBag.Error = "Select an option!";
                return View();

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

    }
}
