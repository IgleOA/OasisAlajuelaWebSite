﻿using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private RolesBL RBL = new RolesBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL UBL = new UsersBL();

        public ActionResult Index()
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                ViewBag.WriteRight = validation.WriteRight;
                var data = RBL.List();
                return View(data.ToList());
            }
        }

        public ActionResult AddNew()
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
                Roles role = new Roles();
                return View(role);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Roles detail)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();
                var r = RBL.AddNew(detail, InsertUser);
                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    detail.ActionType = "CREATE";
                    return View(detail);
                }
            }
            else
            {
                return View(detail);
            }
        }
    }
}