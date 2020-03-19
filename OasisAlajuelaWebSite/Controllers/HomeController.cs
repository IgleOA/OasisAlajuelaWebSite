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
        private SermonsBL SBL = new SermonsBL();
        private ServicesBL SVCBL = new ServicesBL();

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
            var lastEvent = UBL.List(DateTime.Today,false).Take(1).FirstOrDefault();

            return View(lastEvent);
        }

        public ActionResult _Services()
        {
            var svcs = SVCBL.List(true);

            return View(svcs.ToList());
        }

        public ActionResult _Sermons()
        {
            var data = (from d in SBL.List()
                       where d.ActiveFlag == true
                       orderby d.SermonDate descending
                       select d).Take(3);

            return View(data.ToList());
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