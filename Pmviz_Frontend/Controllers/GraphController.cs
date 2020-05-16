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
        public async Task<IActionResult> Index()
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
                    else
                    {
                        using (response = await httpClient.GetAsync("http://localhost:8080/api/moulds"))
                        {
                            ViewData["moulds"] = response.Content.ReadAsStringAsync().Result;
                            status = response.IsSuccessStatusCode;
                            if (status == false)
                            {
                                return RedirectToAction("Index", "Home", new { error = "1" });
                            }
                            else
                            {
                                return View();
                            }
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> ConformanceGraph(string process, string typeCompare, string miner, string moulds, string startDate, string endDate, string threshold)
        {
            string json, url;
            if(typeCompare == "moulds")
            {
                json = "{ \"moulds\":" + moulds.ToString() + "}";
            }
            else
            {
                json = "{ \"startDate\":\"" + startDate + "\", \"endDate\":\"" + endDate + "\"}";
            }
            if (miner == "alpha-miner")
            {
                url = "http://localhost:8080/api/conformance/performance/" + miner + "/" + process;
            }
            else
            {
                url = "http://localhost:8080/api/conformance/performance/" + miner + "/" + process + "?threshold=" + threshold.Replace(',', '.');
            }
            HttpResponseMessage response;
            HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.PostAsync(url, c))
                {
                    ViewData["conformance"] = response.Content.ReadAsStringAsync().Result;
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                    else
                    {
                        var obj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                        json = "{ \"nodes\":" + JsonConvert.SerializeObject(obj["nodes"]) + "}";

                        c = new StringContent(json, Encoding.UTF8, "application/json");

                        response = await httpClient.PostAsync("http://localhost:8080/api/conformance/process/" + process, c);
                        status = response.IsSuccessStatusCode;
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

        public async Task<IActionResult> GetFilter(string process, string moulds, string resources, string activities, string startDate, string endDate, string nodes)
        {
            string json = "{ \"nodes\":" + nodes.ToString();
            if (moulds != null)
            {
                json += ", \"moulds\":" + moulds.ToString();
            }
            if (resources != null)
            {
                json += ", \"resources\":" + resources.ToString();
            }
            if (activities != null)
            {
                json += ", \"activities\":" + activities.ToString();
            }
            if (startDate != null && endDate != null)
            {
                json += ", \"startDate\":\"" + startDate + "\", \"endDate\":\"" + endDate + "\"";
            }
            json += " }";
            HttpResponseMessage response;
            HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.PostAsync("http://localhost:8080/api/conformance/process/" + process, c))
                {
                    var status = response.IsSuccessStatusCode;
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
        public async Task<IActionResult> GetActivities(int process)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + process + "/activities"))
                {
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return Json(new { success = false, request = response.Content.ReadAsStringAsync() });
                    }
                    else
                    {
                        var obj = response.Content.ReadAsStringAsync().Result;
                        response = await httpClient.GetAsync("http://localhost:8080/api/processes/" + process + "/resources");
                        return Json(new { success = true, request = new { activities = obj, resources = response.Content.ReadAsStringAsync().Result } });
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

    }
}