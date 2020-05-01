using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pmviz_Frontend.Models;

namespace Pmviz_Frontend.Controllers
{
    public class TagController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                // GET ALL MOULDS
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/moulds"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var moulds = JsonConvert.DeserializeObject<List<Mould>>(apiResponse);
                        ViewData["moulds"] = moulds;
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Moulds not available. Please try again later.";
                        return View();
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(int mouldSelected)
        {
            using (var httpClient = new HttpClient())
            {
                // GET ALL MOULDS
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/moulds"))
                {
                    var moulds = new List<Mould>();
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        moulds = JsonConvert.DeserializeObject<List<Mould>>(apiResponse);
                        ViewData["moulds"] = moulds;
                    } else
                    {
                        ViewBag.Error = "Moulds not available. Please try again later.";
                    }
                }
                // GET ALL PARTS FROM THAT MOULD
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/moulds/" + mouldSelected + "/parts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var parts = JsonConvert.DeserializeObject<List<Part>>(apiResponse);
                        ViewData["parts"] = parts;
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorPart = "Parts not available. Please try again later.";
                        return View();
                    }
                }
            }



        }

    }
}