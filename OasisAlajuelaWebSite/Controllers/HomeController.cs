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
        private BannersBL BBL = new BannersBL();
        private UpcommingEventsBL UBL = new UpcommingEventsBL();

        public ActionResult Index()
        {
            HomePage Home = HBL.Home();
            return View(Home);
            
        }

        public ActionResult _Banners()
        {
            var banners = BBL.Banners("HomePage",true);

            return View(banners.ToList());
        }

        public ActionResult _UpcommingEvents()
        {
            var lastEvent = UBL.List(DateTime.Today).Where(x => x.Order == 1).FirstOrDefault();

            return View(lastEvent);
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