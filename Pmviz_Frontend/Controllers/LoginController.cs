using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pmviz_Frontend.Models;

namespace Pmviz_Frontend.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            User user = new User
            {
                Username = username,
                Password = password
            };
            var content = new StringContent(JsonConvert.SerializeObject(user).ToString(), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync("http://localhost:8080/api/login/token", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if(status == true)
                    {
                        var obj = JObject.Parse(apiResponse);
                        var token = (string)obj["token"];
                        System.Diagnostics.Debug.WriteLine("OI MANOS");

                        //store token on session storage
                        HttpContext.Session.SetString("sessionKey", token);
                        
                        return View("Views/Home/Index.cshtml");
                    }
                    else
                    {
                        if(response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            ViewBag.Error = "Invalid Credentials!";
                            return View();
                        }
                    }
                    ViewBag.Error = "Something went wrong. Please try again later.";
                    return View();

                }
            }
        }
    }
}