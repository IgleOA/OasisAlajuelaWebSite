using System;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using OasisAlajuelaWebSite.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Configuration;

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
        private UsersBL UsBL = new UsersBL();
        private RightsBL RRBL = new RightsBL();
        private UserNotesBL UNBL = new UserNotesBL();
        private BlogsBL PBL = new BlogsBL();

        public ActionResult Index()
        {
            HomePage Home = HBL.Home();
            if (Request.IsAuthenticated)
            {
                UsBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));

                List<UserNotes> Notes = UNBL.List(User.Identity.GetUserName(), false);
                
                if(Notes.Count() > 0)
                {
                    ViewBag.Note = true;
                }
            }
            List<Blogs> Casts = PBL.List();
            if(Casts.Count() > 0)
            {
                ViewBag.Blogs = true;
            }
            return View(Home);
        }

        public ActionResult About()
        {
            AboutPage Aboutpage = ABL.About();

            if (Request.IsAuthenticated)
            {
                UsBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
                if (validation.ReadRight == false)
                {
                    ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Users user = UsBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                    if (user.RoleName.Contains("Admin"))
                    {
                        ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                    }
                    else
                    {
                        ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                    }
                    ViewBag.Write = validation.WriteRight;
                    return View(Aboutpage);
                }
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View(Aboutpage);
            }
        }

        public ActionResult History()
        {
            if (Request.IsAuthenticated)
            {
                Users user = UsBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }
                return View();
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View();
            }
        }

        public ActionResult _Banners()
        {
            var banners = BBL.Banners("HomePage",true);

            return View(banners.ToList());
        }

        public ActionResult _UpcommingEvents()
        {
            var lastEvent = UBL.List(DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])),false,true).Take(1).FirstOrDefault();

            return View(lastEvent);
        }

        public ActionResult _Services()
        {
            var svcs = SVCBL.List(true);

            return View(svcs.ToList());
        }
        public ActionResult _Sermons()
        {
            var data = (from d in SBL.List(true)
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
            var data = NBL.List(true).Where(x => x.ShowFlag == true).Take(3);

            return View(data.ToList());
        }

        

        public ActionResult _Menu()
        {
            var menu = WBL.WDByUser(User.Identity.GetUserName());

            return View(menu.ToList());
        }

        [HandleError]
        public ActionResult Error()
        {
            return View();
        }
    }
}