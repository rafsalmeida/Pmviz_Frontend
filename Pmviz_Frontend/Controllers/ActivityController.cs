using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pmviz_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Controllers
{
    public class ActivityController : Controller
    {
        public async Task<IActionResult> Index([FromQuery(Name = "id")] string logid)
        {

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/activity-frequency/processes/"+logid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true) {
                        var activityList = JsonConvert.DeserializeObject<List<ActivityFreq>>(apiResponse);
                        foreach(var a in activityList)
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
                        if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            ViewBag.Error = await response.Content.ReadAsStringAsync();
                            return View();

                        }
                        ViewBag.Error = "Error retrieving statistics. Please, try again later.";
                        return View();

                    }
                }
            }
        }
    }
}