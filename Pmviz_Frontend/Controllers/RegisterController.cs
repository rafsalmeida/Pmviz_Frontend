using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pmviz_Frontend.Models;

namespace Pmviz_Frontend.Controllers
{
    public class RegisterController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/users/roles"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var roles = JsonConvert.DeserializeObject<List<String>>(apiResponse);
                        ViewData["roles"] = roles;

                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Algo deu errado.";
                        return View();
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string name, string email, string role, string password, string confirmpassword)
        {

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/users/roles"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        var roles = JsonConvert.DeserializeObject<List<String>>(apiResponse);
                        ViewData["roles"] = roles;
                    }
                    else
                    {
                        ViewBag.Error = "Algo deu errado.";
                    }
                }
                
                if (!password.Equals(confirmpassword))
                {
                    ViewBag.Error = "A password e a sua confirmação não correspondem.";
                    return View();

                }
                User user = new User
                {
                    Username = username,
                    Name = name,
                    Email = email,
                    Role = role,
                    Password = password
                };

                var content = new StringContent(JsonConvert.SerializeObject(user).ToString(), Encoding.UTF8, "application/json");


                using (var response = await httpClient.PostAsync("http://localhost:8080/api/users", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        ViewBag.Success = "Utilizador "+ username + " criado!";
                        return View();
                    }
                    else
                    {
                        if (response.StatusCode == HttpStatusCode.Conflict)
                        {
                            ViewBag.Error = "Email e username devem ser únicos!";
                            return View();
                        }
                    }

                    ViewBag.Error = "Algo deu errado.";
                    return View();

                }
            }
        }
    }
}