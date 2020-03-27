using System;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Linq;
using PagedList;

namespace OasisAlajuelaWebSite.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        private HomeBL HBL = new HomeBL();
        private BannersBL BBL = new BannersBL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HomePage()
        {
            HomePage HP = HBL.Home();

            return View(HP);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HomePage(HomePage HP)
        {
            string InsertUser = User.Identity.GetUserName();

            var r = HBL.AddHomePage(HP, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado en su solicitud, por favor intente nuevamente.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                HP.ActionType = "CREATE";
                return View(HP);
            }
        }

        public ActionResult Banners(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LocationOrder = String.IsNullOrEmpty(sortOrder) ? "location_desc" : "";
            ViewBag.StatusOrder = sortOrder == "Active" ? "Desactive" : "Active";

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var banners = from b in BBL.Banners(null, null)
                          select b;

            if(!String.IsNullOrEmpty(searchString))
            {
                banners = banners.Where(b => b.BannerName.Contains(searchString));
            }
            switch(sortOrder)
            {
                case "location_desc": banners = banners.OrderByDescending(b => b.LocationBanner);
                    break;
                case "Active": banners = banners.OrderBy(b => b.ActiveFlag ? "A" : "B");
                    break;
                case "Desactive": banners = banners.OrderByDescending(b => b.ActiveFlag ? "A" : "B");
                    break;
                default: banners = banners.OrderBy(b => b.LocationBanner).OrderBy(b => b.Order);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(banners.ToPagedList(pageNumber,pageSize));
        }
    }
}