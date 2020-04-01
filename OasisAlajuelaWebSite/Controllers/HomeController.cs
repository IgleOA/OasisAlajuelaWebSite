using System;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using OasisAlajuelaWebSite.Models;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    [AllowSameSite]
    public class HomeController : Controller
    {
        private HomeBL HBL = new HomeBL();
        private BannersBL BBL = new BannersBL();
        private UpcommingEventsBL UBL = new UpcommingEventsBL();
        private SermonsBL SBL = new SermonsBL();
        private ServicesBL SVCBL = new ServicesBL();
        private YouTubeBL YBL = new YouTubeBL();
        private NewsBL NBL = new NewsBL();
        private AboutPageBL ABL = new AboutPageBL();
        private WebDirectoryBL WBL = new WebDirectoryBL();

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
            var lastEvent = UBL.List(DateTime.Now,false,true).Take(1).FirstOrDefault();

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

        public ActionResult _YouTubeVideos()
        {
            var data = (from d in YBL.Youtubelist(5)
                        orderby d.PublishedAt descending
                        select d).Take(3);

            return View(data.ToList());
        }

        public ActionResult _News()
        {
            var data = NBL.List().Take(3);

            return View(data.ToList());
        }

        public ActionResult About()
        {
            AboutPage Aboutpage = ABL.About();

            return View(Aboutpage);
        }

        public ActionResult _Menu()
        {
            var menu = WBL.WDByUser(User.Identity.GetUserName());

            return View(menu.ToList());
        }
    }
}