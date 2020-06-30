using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pmviz_Frontend.Controllers
{
    public class GraphController : Controller
    {
        public async Task<IActionResult> ConformanceGraph()
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.GetAsync("http://localhost:8080/api/processes"))
                {
                    ViewData["processes"] = response.Content.ReadAsStringAsync().Result;
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                    return View();
                }
            }
        }

        public async Task<IActionResult> FrequencyPerformanceGraph()
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.GetAsync("http://localhost:8080/api/processes"))
                {
                    ViewData["processes"] = response.Content.ReadAsStringAsync().Result;
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                    return View();
                }
            }
        }

        public async Task<IActionResult> GetFullGraph(string process, string miner, string moulds, string parts, string activities, string resources, string startDate, string endDate, 
            string threshold, string estimatedEnd, string mouldsFilter, string partsFilter, string activitiesFilter, string resourcesFilter, string startDateFilter, string endDateFilter)
        {
            string json, url;
            json = "{ \"isEstimatedEnd\":" + estimatedEnd;
            if (moulds != null)
            {
                json += ", \"moulds\":" + moulds.ToString();
            }
            if (parts != null)
            {
                json += ", \"parts\":" + parts.ToString();
            }
            if (resources != null)
            {
                json += ", \"resources\":" + resources.ToString();
            }
            if (activities != null)
            {
                json += ", \"activities\":" + activities.ToString();
            }
            if (startDate != null)
            {
                json += ", \"startDate\":\"" + startDate + "\"";
            }
            if (endDate != null)
            {
                json += ", \"endDate\":\"" + endDate + "\"";
            }
            json += " }";

            if (miner == "alpha-miner")
            {
                url = "http://localhost:8080/api/conformance/performance/" + miner + "/model/" + process;
            }
            else
            {
                url = "http://localhost:8080/api/conformance/performance/" + miner + "/model/" + process + "?threshold=" + threshold.Replace(',', '.');
            }
            HttpResponseMessage response;
            HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.PostAsync(url, c))
                {
                    var status = response.IsSuccessStatusCode;
                    if (response.ReasonPhrase == "No Content")
                    {
                        return Json(new { success = true, request = "" });
                    }
                    if (status == false)
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                    else
                    {
                        var obj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        json = "{ \"nodes\":" + JsonConvert.SerializeObject(obj["nodes"]) + ", \"isEstimatedEnd\":" + estimatedEnd;
                        if (mouldsFilter != null)
                        {
                            json += ", \"moulds\":" + mouldsFilter.ToString();
                        }
                        if (partsFilter != null)
                        {
                            json += ", \"parts\":" + partsFilter.ToString();
                        }
                        if (resourcesFilter != null)
                        {
                            json += ", \"resources\":" + resourcesFilter.ToString();
                        }
                        if (activitiesFilter != null)
                        {
                            json += ", \"activities\":" + activitiesFilter.ToString();
                        }
                        if (startDateFilter != null)
                        {
                            json += ", \"startDate\":\"" + startDateFilter + "\"";
                        }
                        if (endDateFilter != null)
                        {
                            json += ", \"endDate\":\"" + endDateFilter + "\"";
                        }
                        json += " }";
                        c = new StringContent(json, Encoding.UTF8, "application/json");

                        response = await httpClient.PostAsync("http://localhost:8080/api/conformance/performance/process/" + process, c);
                        status = response.IsSuccessStatusCode;
                        if (response.ReasonPhrase == "No Content")
                        {
                            return Json(new { success = true, request = "" });
                        }
                        if (status == false)
                        {
                            return Json(new { success = false, request = response.Content.ReadAsStringAsync() });
                        }
                        else
                        {
                            return Json(new { success = true, request = new { baseData = obj, caseData = JObject.Parse(response.Content.ReadAsStringAsync().Result) } });
                        }   
                    }
                }
            }
        }

        public async Task<IActionResult> GetFilter(string process, string moulds, string parts, string resources, string activities, string startDate, string endDate, string estimatedEnd, string nodes)
        {
            string json = "{ \"nodes\":" + nodes.ToString() + ", \"isEstimatedEnd\":" + estimatedEnd;
            if (moulds != null)
            {
                json += ", \"moulds\":" + moulds.ToString();
            }
            if (parts != null)
            {
                json += ", \"parts\":" + parts.ToString();
            }
            if (resources != null)
            {
                json += ", \"resources\":" + resources.ToString();
            }
            if (activities != null)
            {
                json += ", \"activities\":" + activities.ToString();
            }
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
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.PostAsync("http://localhost:8080/api/conformance/performance/process/" + process, c))
                {
                    var status = response.IsSuccessStatusCode;
                    if (response.ReasonPhrase == "No Content")
                    {
                        return Json(new { success = true, request = "" });
                    }
                    if (status == false)
                    {
                        return Json(new { success = false, request = response.Content.ReadAsStringAsync() });
                    }
                    else
                    {
                        return Json(new { success = true, request = JObject.Parse(response.Content.ReadAsStringAsync().Result) });
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetModel(string[] nodes, string [] activities, string[] moulds, string startDate, string endDate, int process)
        {
            string json = "{ \"nodes\":" + JsonConvert.SerializeObject(nodes) + ", \"activities\":" + JsonConvert.SerializeObject(activities) + ", \"moulds\":" + JsonConvert.SerializeObject(moulds);
            if (startDate != null && endDate != null)
            {
                json +=  ", \"startDate\":\"" + startDate + "\", \"endDate\":\"" + endDate + "\"";
            }
            json += "}";
            HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            var asd = await httpClient.PostAsync("http://localhost:8080/api/conformance/process/" + process, c);

            return Json(new{ success = true, request = asd.Content.ReadAsStringAsync() });
        }

        [HttpPost]
        public async Task<IActionResult> GetInformation(int process)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.GetAsync("http://localhost:8080/api/conformance/" + process + "/filterInformation"))
                {
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return Json(new { success = false, request = response.Content.ReadAsStringAsync() });
                    }
                    else
                    {
                        return Json(new { success = true, request = response.Content.ReadAsStringAsync().Result });
                    }
                }
            }
        }

        public async Task<IActionResult> GetConformance(int process)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + process + "/resources"))
                {
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return Json(new { success = false, request = response.Content.ReadAsStringAsync() });
                    }
                    else
                    {
                        return Json(new { success = true, request = response.Content.ReadAsStringAsync() });
                    }
                }
            }
        }

        public async Task<IActionResult> GetWorkFlow(string process, string miner, string threshold)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                string url = "http://localhost:8080/api/workflow-network/" + miner + "/processes/" + process;
                if (miner == "heuristic-miner")
                {
                    url += "?threshold=" + threshold.Replace(',', '.');
                }
                using (response = await httpClient.GetAsync(url))
                {
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return Json(new { success = false, request = response.Content.ReadAsStringAsync() });
                    }
                    return Json(new { success = true, request = JObject.Parse(response.Content.ReadAsStringAsync().Result) });
                }
            }
        }

    }
}