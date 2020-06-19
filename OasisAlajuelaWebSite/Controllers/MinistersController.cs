using ET;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Configuration;
using PagedList;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class MinistersController : Controller
    {
        private MinistersBL MBL = new MinistersBL();
        private UsersBL UBL = new UsersBL();
        private RightsBL RRBL = new RightsBL();

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                ViewBag.WriteRight = validation.WriteRight;

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var list = from b in MBL.List(false)
                           select b;

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.FullName.ToLower().Contains(searchString.ToLower()) | b.Title.ToLower().Contains(searchString.ToLower()));
                }

                ViewBag.UsersCount = list.Count();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult AddNew()
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            Ministers Detail = new Ministers();
            return View(Detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Ministers Detail)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                var r = MBL.AddNew(Detail, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Detail.ActionType = "CREATE";
                    return View(Detail);
                }
            }
            else
            {
                return View(Detail);
            }
        }

        public ActionResult Edit(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Ministers Detail = MBL.Details(id);
                return View(Detail);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ministers Detail)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                var r = MBL.Update(Detail, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Detail.ActionType = "UPDATE";
                    return View(Detail);
                }
            }
            else
            {
                return View(Detail);
            }
        }
    }
}