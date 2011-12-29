using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Facebook;
using System.Text;

namespace SD.FC.MVC.Application.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Facebook Client for C# SDK";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
