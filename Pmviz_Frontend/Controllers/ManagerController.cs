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
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            List<User> usersList = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:8080/api/user"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    usersList = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                }
            }
            return View(usersList);
        }
    }
}