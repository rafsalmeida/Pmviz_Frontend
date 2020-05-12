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

        public async Task<IActionResult> ConformanceGraph(string process, string miner, string moulds, string startDate, string endDate, string threshold)
        {
            string json, url;
            if(moulds != null)
            {
                json = "{ \"moulds\":" + moulds + "}";
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
                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    ViewData["conformance"] = response.Content.ReadAsStringAsync().Result;
                    ViewData["process"] = process;
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                    else
                    {
                        var obj = JObject.Parse(apiResponse);

                        json = "{ \"nodes\":" + JsonConvert.SerializeObject(obj["nodes"]) + "}";

                        c = new StringContent(json, Encoding.UTF8, "application/json");


                        response = await httpClient.PostAsync("http://localhost:8080/api/conformance/process/" + process, c);
                        ViewData["compare"] = await response.Content.ReadAsStringAsync();

                        return View("ConformanceGraph");
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
                        return Json(new { success = true, request = response.Content.ReadAsStringAsync() });
                    }
                }
            }
        }

    }
}