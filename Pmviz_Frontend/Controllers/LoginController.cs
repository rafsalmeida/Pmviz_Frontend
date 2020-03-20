using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pmviz_Frontend.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            System.Diagnostics.Debug.WriteLine($"username: {username} - password : {password}");
            return View();
        }
    }
}