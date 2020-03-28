using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            if(HttpContext.Session.GetString("sessionKey") != null)
            {
                return RedirectToAction("Index", "home");
            }
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

                        //store token on session storage
                        HttpContext.Session.SetString("sessionKey", token);

                        GetUserDetails();


                        return RedirectToAction("Index", "home");
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

        public async void GetUserDetails()
        {
            //define the token as a bearer token
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("sessionKey"));

            using (var response = await client.GetAsync("http://localhost:8080/api/login/claims"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("userDetails",apiResponse);
                  
                    //how to get the role
                    var obj = JObject.Parse(HttpContext.Session.GetString("userDetails"));
                    var role = obj["role"];

                    System.Diagnostics.Debug.WriteLine(role);

                }
                else
                {
                    ViewBag.Error = "Something went wrong. Please try again later.";

                }
            }

        }



    }
}