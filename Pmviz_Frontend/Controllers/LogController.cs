using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
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
                        ViewBag.Error = await response.Content.ReadAsStringAsync();
                        return View();

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
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                    using (var response = await httpClient.GetAsync("http://localhost:8080/api/activities/" + processId+"/freqduration"))
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
                                meanTime += a.MeanDuration.Millis != 0 ? a.MeanDuration.Millis + "ms " : "";
                                a.MeanActivityFormatted = meanTime;

                                var tsMean = new TimeSpan(a.MeanDuration.Days, a.MeanDuration.Hours, a.MeanDuration.Minutes, a.MeanDuration.Seconds, a.MeanDuration.Millis);

                                a.MeanInMinutes = Math.Round(tsMean.TotalMinutes, 0);

                                var medianTime = a.MedianDuration.Days != 0 ? a.MedianDuration.Days + "d " : "";
                                medianTime += a.MedianDuration.Hours != 0 ? a.MedianDuration.Hours + "h " : "";
                                medianTime += a.MedianDuration.Minutes != 0 ? a.MedianDuration.Minutes + "m " : "";
                                medianTime += a.MedianDuration.Seconds != 0 ? a.MedianDuration.Seconds + "s " : "";
                                medianTime += a.MedianDuration.Millis != 0 ? a.MedianDuration.Millis + "ms " : "";
                                a.MedianActivityFormatted = medianTime;

                                var tsMedian = new TimeSpan(a.MedianDuration.Days, a.MedianDuration.Hours, a.MedianDuration.Minutes, a.MedianDuration.Seconds, a.MedianDuration.Millis);

                                a.MedianInMinutes = Math.Round(tsMedian.TotalMinutes, 0);

                                var minTime = a.MinDuration.Days != 0 ? a.MinDuration.Days + "d " : "";
                                minTime += a.MinDuration.Hours != 0 ? a.MinDuration.Hours + "h " : "";
                                minTime += a.MinDuration.Minutes != 0 ? a.MinDuration.Minutes + "m " : "";
                                minTime += a.MinDuration.Seconds != 0 ? a.MinDuration.Seconds + "s " : "";
                                minTime += a.MinDuration.Millis != 0 ? a.MinDuration.Millis + "ms " : "";
                                a.MinActivityFormatted = minTime;

                                var maxTime = a.MaxDuration.Days != 0 ? a.MaxDuration.Days + "d " : "";
                                maxTime += a.MaxDuration.Hours != 0 ? a.MaxDuration.Hours + "h " : "";
                                maxTime += a.MaxDuration.Minutes != 0 ? a.MaxDuration.Minutes + "m " : "";
                                maxTime += a.MaxDuration.Seconds != 0 ? a.MaxDuration.Seconds + "s " : "";
                                maxTime += a.MaxDuration.Millis != 0 ? a.MaxDuration.Millis + "ms " : "";
                                a.MaxActivityFormatted = maxTime;

                                a.RelativeFrequency = Math.Round(a.RelativeFrequency, 2);



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

                            ViewBag.Error = await response.Content.ReadAsStringAsync();
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
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                    using (var response = await httpClient.GetAsync("http://localhost:8080/api/activities/" + processId+"/workhours"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var status = response.IsSuccessStatusCode;
                        if (status == true)
                        {
                            var activityList = JsonConvert.DeserializeObject<List<ActivityEffort>>(apiResponse);

                            foreach (var a in activityList)
                            {
                                var timeSpan = TimeSpan.FromMilliseconds(a.TotalWorkHoursMillis);

                                a.TotalWorkHoursMillis = Math.Round(timeSpan.TotalMinutes, 0);
                                a.FullDate = a.TotalWorkHours.Days != 0 ? a.TotalWorkHours.Days + "d " : "";
                                a.FullDate += a.TotalWorkHours.Hours != 0 ? a.TotalWorkHours.Hours + "h " : "";
                                a.FullDate += a.TotalWorkHours.Minutes != 0 ? a.TotalWorkHours.Minutes + "m " : "";
                                a.FullDate += a.TotalWorkHours.Seconds != 0 ? a.TotalWorkHours.Seconds + "s " : "";
                                a.FullDate += a.TotalWorkHours.Millis != 0 ? a.TotalWorkHours.Millis + "ms " : " 0ms";
                            }

                            IEnumerable<ActivityEffort>  al = activityList.OrderByDescending(x => x.TotalWorkHoursMillis).ThenBy(x => x.Activity).ToList();
                            ViewData["WorkTime"] = al;


                            return View();
                        }
                        else
                        {
                            ViewBag.Error = await response.Content.ReadAsStringAsync();

                            return View();

                        }
                    }
                }
                #endregion
            }
            else if(type == "operational")
            {
                #region Operational 
                ViewData["type"] = "operational";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                    using (var response = await httpClient.GetAsync("http://localhost:8080/api/activities/" + processId + "/operationalhours"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var status = response.IsSuccessStatusCode;
                        if (status == true)
                        {
                            var activityList = JsonConvert.DeserializeObject<List<ActivityOperational>>(apiResponse);
                            if(activityList.Count() == 0)
                            {
                                ViewBag.Error = "Sem eventos associados.";
                                return View();
                            }
                            foreach (var a in activityList)
                            {
                                var timeSpan = TimeSpan.FromMilliseconds(a.TotalOperationalHoursMillis);

                                a.TotalOperationalHoursMillis = Math.Round(timeSpan.TotalMinutes, 0);
                                a.FullDate = a.TotalOperationalHours.Days != 0 ? a.TotalOperationalHours.Days + "d " : "";
                                a.FullDate += a.TotalOperationalHours.Hours != 0 ? a.TotalOperationalHours.Hours + "h " : "";
                                a.FullDate += a.TotalOperationalHours.Minutes != 0 ? a.TotalOperationalHours.Minutes + "m " : "";
                                a.FullDate += a.TotalOperationalHours.Seconds != 0 ? a.TotalOperationalHours.Seconds + "s " : "";
                                a.FullDate += a.TotalOperationalHours.Millis != 0 ? a.TotalOperationalHours.Millis + "ms " : " 0ms";
                            }

                            IEnumerable<ActivityOperational> al = activityList.OrderByDescending(x => x.TotalOperationalHoursMillis).ThenBy(x => x.Activity).ToList();
                            ViewData["OperationalTime"] = al;

                            

                            return View();
                        }
                        else
                        {
                            ViewBag.Error = await response.Content.ReadAsStringAsync();
                            return View();

                        }
                    }
                }
                #endregion
            }
            else
            {
                ViewBag.Error = "Selecione uma opção!";
                return View();

            }

           

        }

        public async Task<IActionResult> Resource([FromQuery(Name = "id")]string processId)
        {
            ViewData["processId"] = processId;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        // get the list of activities
                        var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                        ViewData["activities"] = activityList;
                        ViewData["processId"] = processId;
                    }
                    else
                    {
                        ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                    }
                }

                using (var response = await httpClient.GetAsync("http://localhost:8080/api/workstations"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        // get the list of workstations
                        var workstationList = JsonConvert.DeserializeObject<List<Workstation>>(apiResponse);
                        ViewData["workstations"] = workstationList;
                        ViewData["processId"] = processId;
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorWorkstations = await response.Content.ReadAsStringAsync();
                        return View();
                    }
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> Resource([FromQuery(Name = "id")]string processId, string type, string typeR, string activity, string workstation)
        {
            ViewData["processId"] = processId;
            if(typeR == "user")
            {
                ViewData["typeResource"] = "user";
                if (type == "mean")
                {
                    #region Mean
                    ViewData["type"] = "mean";

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of activities
                                var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                                ViewData["activities"] = activityList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                            }
                        }

                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/workstations"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of workstations
                                var workstationList = JsonConvert.DeserializeObject<List<Workstation>>(apiResponse);
                                ViewData["workstations"] = workstationList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorWorkstations = await response.Content.ReadAsStringAsync();

                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/" + processId + "/users/performance?activity=" + activity))
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
                                    ViewBag.ErrorActivity = "Sem trabalho registado nesta atividade.";
                                    return View();
                                }


                                var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["meanMillis"].ToString()));
                                ViewData["meanMillis"] = Math.Round(timeSpan.TotalMinutes, 0);

                                var allResources = JsonConvert.DeserializeObject<List<ResourceStat>>(data["resources"].ToString());
                                foreach (var r in allResources)
                                {
                                    var timeSpanR = TimeSpan.FromMilliseconds(r.MeanMillis);

                                    r.MeanMillis = Math.Round(timeSpanR.TotalMinutes, 0);
                                    r.GeneralMean = Math.Round(timeSpanR.TotalMinutes, 0);
                                }

                                allResources = allResources.OrderByDescending(x => x.MeanMillis).ThenBy(x => x.Resource).ToList();

                                ViewData["allResources"] = allResources;



                                return View();
                            }
                            else
                            {
                                ViewData["activity"] = activity;

                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

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
                        httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of activities
                                var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                                ViewData["activities"] = activityList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/workstations"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of workstations
                                var workstationList = JsonConvert.DeserializeObject<List<Workstation>>(apiResponse);
                                ViewData["workstations"] = workstationList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorWorkstations = await response.Content.ReadAsStringAsync();

                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/" + processId + "/users/workhours/activities?activity=" + activity))
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

                                var resources = JArray.Parse(data["users"].ToString());
                                if (resources.Count == 0)
                                {
                                    ViewBag.ErrorActivity = "Sem trabalho registado nesta atividade.";
                                    return View();
                                }


                                var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["totalWorkHoursMillis"].ToString()));
                                ViewData["totalWorkHoursMillis"] = Math.Round(timeSpan.TotalMinutes, 0);

                                var allResources = JsonConvert.DeserializeObject<List<ResourceEffort>>(data["users"].ToString());
                                foreach (var r in allResources)
                                {
                                    var timeSpanR = TimeSpan.FromMilliseconds(r.WorkHoursMillis);

                                    r.WorkHoursMillis = Math.Round(timeSpanR.TotalMinutes, 0);
                                }

                                allResources = allResources.OrderByDescending(x => x.WorkHoursMillis).ThenBy(x => x.Username).ToList();

                                ViewData["allResourcesEffort"] = allResources;



                                return View();
                            }
                            else
                            {
                                ViewData["activity"] = activity;

                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                                return View();

                            }
                        }

                    }


                    #endregion


                } 
                else if(type == "effortWorkstation")
                {
                    #region Effort workstation
                    ViewData["type"] = "effortWorkstation";
                    ViewData["workstation"] = workstation;

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of activities
                                var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                                ViewData["activities"] = activityList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();
                                ;
                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/workstations"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of workstations
                                var workstationList = JsonConvert.DeserializeObject<List<Workstation>>(apiResponse);
                                ViewData["workstations"] = workstationList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorWorkstations = await response.Content.ReadAsStringAsync();

                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/" + processId + "/users/workhours/workstations?workstation=" + workstation))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                var data = JObject.Parse(apiResponse);

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

                                var resources = JArray.Parse(data["users"].ToString());
                                if (resources.Count == 0)
                                {
                                    ViewBag.ErrorActivity = "Sem trabalho registado nessa estação.";
                                    return View();
                                }


                                var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["totalWorkHoursMillis"].ToString()));
                                ViewData["totalWorkHoursMillis"] = Math.Round(timeSpan.TotalMinutes, 0);

                                var allResources = JsonConvert.DeserializeObject<List<ResourceEffort>>(data["users"].ToString());
                                foreach (var r in allResources)
                                {
                                    var timeSpanR = TimeSpan.FromMilliseconds(r.WorkHoursMillis);

                                    r.WorkHoursMillis = Math.Round(timeSpanR.TotalMinutes, 0);
                                }

                                allResources = allResources.OrderByDescending(x => x.WorkHoursMillis).ThenBy(x => x.Username).ToList();

                                ViewData["allResourcesEffort"] = allResources;



                                return View();
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

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
                        httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of activities
                                var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                                ViewData["activities"] = activityList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                                {
                                    ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                                }
                                ViewBag.ErrorActivity = "Algo deu errado.";
                            }
                        }

                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/workstations"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of workstations
                                var workstationList = JsonConvert.DeserializeObject<List<Workstation>>(apiResponse);
                                ViewData["workstations"] = workstationList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                                {
                                    ViewBag.ErrorWorkstations = await response.Content.ReadAsStringAsync();

                                }
                                ViewBag.ErrorWorkstations = "Algo deu errado.";
                            }
                        }
                    }


                    ViewBag.Error = "Selecione uma opção!";
                    return View();

                }
            } else if (typeR == "workstation")
            {
                ViewData["typeResource"] = "workstation";

                if (type == "meanWork")
                {
                    #region Mean
                    ViewData["type"] = "meanWork";
                    ViewData["activity"] = activity;
                    ViewData["hasValues"] = "true";
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of activities
                                var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                                ViewData["activities"] = activityList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/workstations"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of workstations
                                var workstationList = JsonConvert.DeserializeObject<List<Workstation>>(apiResponse);
                                ViewData["workstations"] = workstationList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorWorkstations = await response.Content.ReadAsStringAsync();

                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/" + processId + "/workstations/performance?activity="+activity))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                var workstationList = JsonConvert.DeserializeObject<List<WorkstationFreq>>(apiResponse);
                                foreach (var a in workstationList)
                                {
                                    var meanTime = a.MeanDuration.Days != 0 ? a.MeanDuration.Days + "d " : "";
                                    meanTime += a.MeanDuration.Hours != 0 ? a.MeanDuration.Hours + "h " : "";
                                    meanTime += a.MeanDuration.Minutes != 0 ? a.MeanDuration.Minutes + "m " : "";
                                    meanTime += a.MeanDuration.Seconds != 0 ? a.MeanDuration.Seconds + "s " : "";
                                    meanTime += a.MeanDuration.Millis != 0 ? a.MeanDuration.Millis + "ms " : "";
                                    a.MeanActivityFormatted = meanTime;

                                    var tsMean = new TimeSpan(a.MeanDuration.Days, a.MeanDuration.Hours, a.MeanDuration.Minutes, a.MeanDuration.Seconds, a.MeanDuration.Millis);

                                    a.MeanInMinutes = Math.Round(tsMean.TotalMinutes, 0);

                                    var medianTime = a.MedianDuration.Days != 0 ? a.MedianDuration.Days + "d " : "";
                                    medianTime += a.MedianDuration.Hours != 0 ? a.MedianDuration.Hours + "h " : "";
                                    medianTime += a.MedianDuration.Minutes != 0 ? a.MedianDuration.Minutes + "m " : "";
                                    medianTime += a.MedianDuration.Seconds != 0 ? a.MedianDuration.Seconds + "s " : "";
                                    medianTime += a.MedianDuration.Millis != 0 ? a.MedianDuration.Millis + "ms " : "";
                                    a.MedianActivityFormatted = medianTime;

                                    var tsMedian = new TimeSpan(a.MedianDuration.Days, a.MedianDuration.Hours, a.MedianDuration.Minutes, a.MedianDuration.Seconds, a.MedianDuration.Millis);

                                    a.MedianInMinutes = Math.Round(tsMedian.TotalMinutes, 0);

                                    var minTime = a.MinDuration.Days != 0 ? a.MinDuration.Days + "d " : "";
                                    minTime += a.MinDuration.Hours != 0 ? a.MinDuration.Hours + "h " : "";
                                    minTime += a.MinDuration.Minutes != 0 ? a.MinDuration.Minutes + "m " : "";
                                    minTime += a.MinDuration.Seconds != 0 ? a.MinDuration.Seconds + "s " : "";
                                    minTime += a.MinDuration.Millis != 0 ? a.MinDuration.Millis + "ms " : "";
                                    a.MinActivityFormatted = minTime;

                                    var maxTime = a.MaxDuration.Days != 0 ? a.MaxDuration.Days + "d " : "";
                                    maxTime += a.MaxDuration.Hours != 0 ? a.MaxDuration.Hours + "h " : "";
                                    maxTime += a.MaxDuration.Minutes != 0 ? a.MaxDuration.Minutes + "m " : "";
                                    maxTime += a.MaxDuration.Seconds != 0 ? a.MaxDuration.Seconds + "s " : "";
                                    maxTime += a.MaxDuration.Millis != 0 ? a.MaxDuration.Millis + "ms " : "";
                                    a.MaxActivityFormatted = maxTime;

                                    a.RelativeFrequency = Math.Round(a.RelativeFrequency, 2);


                                }
                                
                                IEnumerable<WorkstationFreq> al = workstationList.OrderByDescending(x => x.Frequency).ThenBy(x => x.Workstation).ToList();
                                ViewData["Frequency"] = al;
                                al = workstationList.OrderByDescending(x => x.MeanInMinutes).ThenBy(x => x.Workstation).ToList();
                                ViewData["Mean"] = al;
                                al = workstationList.OrderByDescending(x => x.MedianInMinutes).ThenBy(x => x.Workstation).ToList();
                                ViewData["Median"] = al;

                                return View(workstationList);
                            }
                            else
                            {
                                ViewBag.Error = await response.Content.ReadAsStringAsync();

                                return View();

                            }
                        }

                    }


                    #endregion

                }
                else if (type == "effortWork")
                {
                    #region Effort
                    ViewData["type"] = "effortWork";
                    ViewData["activity"] = activity;

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of activities
                                var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                                ViewData["activities"] = activityList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();
                                
                            }
                        }

                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/workstations"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of workstations
                                var workstationList = JsonConvert.DeserializeObject<List<Workstation>>(apiResponse);
                                ViewData["workstations"] = workstationList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorWorkstations = await response.Content.ReadAsStringAsync();

                            }
                        }
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/resources/" + processId + "/workstations/operationalhours?activity=" + activity))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                var data = JObject.Parse(apiResponse);

                                ViewData["activity"] = activity;
                                ViewData["hasValues"] = "true";

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

                                var resources = JArray.Parse(data["workstations"].ToString());
                                if (resources.Count == 0)
                                {
                                    ViewBag.ErrorActivity = "Sem trabalho registado nesta atividade.";
                                    return View();
                                }


                                var timeSpan = TimeSpan.FromMilliseconds(Double.Parse(data["totalOperationalHoursMillis"].ToString()));
                                ViewData["totalWorkHoursMillis"] = timeSpan.TotalMinutes;

                                var allResources = JsonConvert.DeserializeObject<List<WorkstationEffort>>(data["workstations"].ToString());
                                foreach (var r in allResources)
                                {
                                    var timeSpanR = TimeSpan.FromMilliseconds(r.OperationalHoursMillis);

                                    r.OperationalHoursMillis = Math.Round(timeSpanR.TotalMinutes, 0);
                                }

                                allResources = allResources.OrderByDescending(x => x.OperationalHoursMillis).ThenBy(x => x.WorkstationName).ToList();

                                ViewData["allResourcesEffort"] = allResources;



                                return View();
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

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
                        httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                        using (var response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + processId + "/activities"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var status = response.IsSuccessStatusCode;
                            if (status == true)
                            {
                                // get the list of activities
                                var activityList = JsonConvert.DeserializeObject<List<Activity>>(apiResponse);
                                ViewData["activities"] = activityList;
                                ViewData["processId"] = processId;
                            }
                            else
                            {
                                ViewBag.ErrorActivity = await response.Content.ReadAsStringAsync();

                            }
                        }

                    }


                    ViewBag.Error = "Select an option!";
                    return View();

                }
            }

            return View();
            

        }


        public async Task<IActionResult> GetFilter(string process, string startDate, string endDate, string type, string categoryType, string categoryResource, string activity, string workstation)
        {
            string json = "{ \"isEstimatedEnd\": true";
            string s;
            if (startDate != null)
            {
                json += ", \"startDate\":\"" + startDate + "\"";
            }
            if (endDate != null)
            {
                json += ", \"endDate\":\"" + endDate + "\"";
            }
            json += " }";
            HttpResponseMessage response;
            HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "http://localhost:8080/api/" + type + "/" + process;
            url += "/" + categoryType;
            if (categoryResource != null)
            {
                url += "/" + categoryResource;
            }
            if (activity != null)
            {
                url += "?activity=" + activity;
            }
            else
            {
                url += "?workstation=" + workstation;
            }
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));
                using (response = await httpClient.PostAsync(url, c))
                {
                    var status = response.IsSuccessStatusCode;
                    if (response.ReasonPhrase == "No Content")
                    {
                        return Json(new { success = true, request = "" });
                    }
                    if (status == false)
                    {
                        return Json(new { success = false, request = response.Content.ReadAsStringAsync().Result });
                    }
                    else
                    {
                        if (categoryResource != null)
                        {
                            if (categoryResource == "performance")
                            {
                                s = response.Content.ReadAsStringAsync().Result.Replace("resource", "Resource").Replace("meanMillis", "MeanMillis");
                                return Json(new { success = true, request = s });
                            }
                            if(categoryResource == "workhours/activities" || categoryResource == "workhours/workstations")
                            {
                                s = response.Content.ReadAsStringAsync().Result.Replace("username", "Username").Replace("workHoursMillis", "WorkHoursMillis").Replace("users", "Users");
                                return Json(new { success = true, request = s });
                            }
                            if (categoryResource == "workstations/operationalhours")
                            {
                                s = response.Content.ReadAsStringAsync().Result.Replace("workstationName", "WorkstationName").Replace("operationalHoursMillis", "OperationalHoursMillis").Replace("workstations", "Workstations"); ;
                                return Json(new { success = true, request = s });
                            }
                        }

                        s = response.Content.ReadAsStringAsync().Result.Replace("activity", "Activity").Replace("totalOperationalHoursMillis", "TotalOperationalHoursMillis").Replace("totalWorkHoursMillis", "TotalWorkHoursMillis");
                        return Json(new { success = true, request = s });
                    }
                }
            }
        }
    }
}
