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

        public async Task<IActionResult> ConformanceGraph(string processes, string miner)
        {
            int[] arr = JsonConvert.DeserializeObject<int[]>(processes);
            int firstProcess = arr[0];
            int lastProcess = arr[1];
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (response = await httpClient.GetAsync("http://localhost:8080/api/conformance/performance/" + miner + "/" + firstProcess + "/with/" + lastProcess))
                {
                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    ViewData["conformance"] = response.Content.ReadAsStringAsync().Result;
                    ViewData["baseProcess"] = firstProcess;
                    ViewData["caseProcess"] = lastProcess;
                    var status = response.IsSuccessStatusCode;
                    if (status == false)
                    {
                        return RedirectToAction("Index", "Home", new { error = "1" });
                    }
                    else
                    {
                        var obj = JObject.Parse(apiResponse);

                        var json = "{ \"nodes\":" + Newtonsoft.Json.JsonConvert.SerializeObject(obj["nodes"]) + "}";

                        HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");


                        var asd = await httpClient.PostAsync("http://localhost:8080/api/conformance/process/" + firstProcess, c);
                        ViewData["compare"] = await asd.Content.ReadAsStringAsync();

                        return View("ConformanceGraph");

                        }
                    }
                }
            }

        [HttpPost]
        public async Task<IActionResult> GetModel(string [] activities, string[] moulds, string startDate, string endDate, int process)
        {
            var json = "";
            if (startDate != null && endDate != null)
            {
                json = "{ \"nodes\":" + JsonConvert.SerializeObject(activities) + ", \"moulds\":" + JsonConvert.SerializeObject(moulds) + ", \"startDate\":\"" + startDate + "\", \"endDate\":\"" + endDate + "\"}";
            }
            else
            {
                json = "{ \"nodes\":" + JsonConvert.SerializeObject(activities) + ", \"moulds\":" + JsonConvert.SerializeObject(moulds) + "}";
            }

            HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            var asd = await httpClient.PostAsync("http://localhost:8080/api/conformance/process/" + process, c);

            return Json(new{ success = true, request = asd.Content.ReadAsStringAsync() });
        }

    }
}