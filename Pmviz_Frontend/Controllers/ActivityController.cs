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
                        foreach(var a in activityList)
                        {
                            var meanTime = a.MeanDuration.Days != 0 ? a.MeanDuration.Days + "d " : "";
                            meanTime += a.MeanDuration.Hours != 0 ? a.MeanDuration.Hours + "h " : "";
                            meanTime += a.MeanDuration.Minutes != 0 ? a.MeanDuration.Minutes + "m " : "";
                            meanTime += a.MeanDuration.Seconds != 0 ? a.MeanDuration.Seconds + "s " : "";
                            meanTime += a.MeanDuration.Millis != 0 ? a.MeanDuration.Millis + "ms " : " 0ms";
                            a.MeanActivityFormatted = meanTime;

                            var medianTime = a.MedianDuration.Days != 0 ? a.MedianDuration.Days + "d " : "";
                            medianTime += a.MedianDuration.Hours != 0 ? a.MedianDuration.Hours + "h " : "";
                            medianTime += a.MedianDuration.Minutes != 0 ? a.MedianDuration.Minutes + "m " : "";
                            medianTime += a.MedianDuration.Seconds != 0 ? a.MedianDuration.Seconds + "s " : "";
                            medianTime += a.MedianDuration.Millis != 0 ? a.MedianDuration.Millis + "ms " : " 0ms";
                            a.MedianActivityFormatted = medianTime;

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
                        return View(activityList);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });

                    }
                }
            }
        }
    }
}