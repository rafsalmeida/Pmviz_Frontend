using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pmviz_Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return RedirectToAction("Index","login");
            try
            {
                return View();

            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Apanhei te");
                return RedirectToAction("Index", "login");
            }
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
