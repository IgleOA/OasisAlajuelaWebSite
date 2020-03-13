using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;

namespace OasisAlajuelaWebSite.Controllers
{
    public class HomeController : Controller
    {
        private HomeBL HBL = new HomeBL();

        public ActionResult Index()
        {
            //HomePage home = new HomePage();

            //home.MainBanners = HBL.Banners("HomePage");

            //return View(home);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}