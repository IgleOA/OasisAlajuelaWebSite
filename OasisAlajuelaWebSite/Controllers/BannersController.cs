﻿using System;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Linq;
using PagedList;
using System.IO;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class BannersController : Controller
    {
        private BannersBL BBL = new BannersBL();
        private BannersLocationBL BLBL = new BannersLocationBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL UBL = new UsersBL();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
                ViewBag.CurrentSort = sortOrder;
                ViewBag.LocationOrder = String.IsNullOrEmpty(sortOrder) ? "location_desc" : "";
                ViewBag.StatusOrder = sortOrder == "Active" ? "Desactive" : "Active";

                if (searchString != null)
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

                if (!String.IsNullOrEmpty(searchString))
                {
                    banners = banners.Where(b => b.BannerName.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "location_desc":
                        banners = banners.OrderByDescending(b => b.LocationBanner);
                        break;
                    case "Active":
                        banners = banners.OrderBy(b => b.ActiveFlag ? "A" : "B");
                        break;
                    case "Desactive":
                        banners = banners.OrderByDescending(b => b.ActiveFlag ? "A" : "B");
                        break;
                    default:
                        banners = banners.OrderBy(b => b.LocationBanner);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                ViewBag.Write = validation.WriteRight;
                return View(banners.ToPagedList(pageNumber, pageSize));
            }
        }

        [Authorize]
        public ActionResult BannersChangeStatus(int id)
        {

            string InsertUser = User.Identity.GetUserName();

            var r = BBL.Update(id, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
                return this.RedirectToAction("Index", "Banners");
            }
        }

        [Authorize]
        public ActionResult AddNew()
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Banner NewBanner = new Banner();

                NewBanner.LList = BLBL.List();

                return View(NewBanner);
            }
        }

        [HttpPost]
        public ActionResult AddNew(Banner MS)
        {

            String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

            MS.BannerExt = FileExt;

            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
            {
                Stream str = MS.file.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                MS.BannerData = FileDet;

                string InsertUser = User.Identity.GetUserName();

                var r = BBL.AddNew(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
                    MS.ActionType = "CREATE";
                    MS.LList = BLBL.List();
                    return View(MS);
                }
            }
            else
            {
                MS.LList = BLBL.List();
                return View(MS);
            }
        }
    }
}