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
                            var meanTime = a.MeanDuration.Days + "d " + a.MeanDuration.Hours + "h "
                                + a.MeanDuration.Minutes + "m " + a.MeanDuration.Seconds + "s " + a.MeanDuration.Millis + "ms ";
                            a.MeanActivityFormatted = meanTime;

                            var medianTime = a.MedianDuration.Days + "d " + a.MedianDuration.Hours + "h "
                                + a.MedianDuration.Minutes + "m " + a.MedianDuration.Seconds + "s " + a.MedianDuration.Millis + "ms ";
                            a.MedianActivityFormatted = medianTime;

                            var minTime = a.MinDuration.Days + "d " + a.MinDuration.Hours + "h "
                                 + a.MinDuration.Minutes + "m " + a.MinDuration.Seconds + "s " + a.MinDuration.Millis + "ms ";
                            a.MinActivityFormatted = minTime;

                            var maxTime = a.MaxDuration.Days + "d " + a.MaxDuration.Hours + "h "
                               + a.MaxDuration.Minutes + "m " + a.MaxDuration.Seconds + "s " + a.MaxDuration.Millis + "ms ";
                            a.MaxActivityFormatted = maxTime;


                        }
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