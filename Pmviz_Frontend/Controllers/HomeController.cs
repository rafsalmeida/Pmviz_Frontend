using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Pmviz_Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromQuery(Name = "error")] string error, [FromQuery(Name = "success")] string success)
        {
            if(error == "1")
            {
                ViewBag.Error = "Algo deu errado..";
            }

            if(success == "1")
            {
                ViewBag.Success = "Mudanças bem sucedidas!";
            }
            /*return RedirectToAction("Index","login");
            var obj = JObject.Parse(HttpContext.Session.GetString("userDetails"));
            var role = obj["role"];
            ViewBag.Role = role;

            TempData["msg"] = "<script>alert('Change succesfully');</script>";
            /*@Html.Raw(TempData["msg"]);*/

            return View();

            
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
