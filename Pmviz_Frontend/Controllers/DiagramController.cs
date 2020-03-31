using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Pmviz_Frontend.Models;

namespace Pmviz_Frontend.Controllers
{
    public class DiagramController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult _Event()
        {
            return Json(Event.Diagram());
        }

        public IActionResult Back()
        {
            var obj = JObject.Parse(HttpContext.Session.GetString("userDetails"));
            var role = obj["role"];

            return RedirectToAction("Index", role.ToString());
        }
    }
}