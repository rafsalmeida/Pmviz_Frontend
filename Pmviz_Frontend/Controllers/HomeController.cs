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

            return View();

            
        }

    }
}
