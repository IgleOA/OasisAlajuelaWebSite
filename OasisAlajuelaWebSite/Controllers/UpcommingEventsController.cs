﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{

    public class UpcommingEventsController : Controller
    {
        private UpcommingEventsBL UBL = new UpcommingEventsBL();
        private MinistersBL MBL = new MinistersBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();

        public ActionResult Index()
        {
            List<UpcommingEvents> list = new List<UpcommingEvents>();

            if (Request.IsAuthenticated)
            {
                USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
                var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
                if (validation.ReadRight == false)
                {
                    ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    var u = from us in USBL.List()
                            where us.UserName == User.Identity.GetUserName()
                            select us;
                    int roleID = u.FirstOrDefault().RoleID;

                    if (roleID == 2 || roleID == 3 || roleID == 4)
                    {
                        ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                        ViewBag.Write = validation.WriteRight;
                        list = UBL.List(DateTime.Now, true, true);
                    }
                    else
                    {
                        ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                        list = UBL.List(DateTime.Now, false, true);
                    }
                    ViewBag.Write = validation.WriteRight;

                    return View(list.ToList());
                }
            }
            else
            {
                list = UBL.List(DateTime.Now, false, true);
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View(list.ToList());
            }
        }

        [Authorize]
        public ActionResult AddNew()
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                UpcommingEvents Event = new UpcommingEvents()
                {
                    MinisterList = MBL.List(true),
                    ScheduledDate = DateTime.Today
                };

                return View(Event);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(UpcommingEvents Event)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                Event.ScheduledDate = Event.ScheduledDate.Add(Event.ScheduledTime);

                var r = UBL.AddNew(Event, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Event.ActionType = "CREATE";
                    Event.MinisterList = MBL.List(true);
                    return View(Event);
                }
            }
            else
            {
                Event.MinisterList = MBL.List(true);
                return View(Event);
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                UpcommingEvents Event = UBL.Details(id);

                Event.MinisterList = MBL.List(true);
                return View(Event);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpcommingEvents Event)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                Event.ScheduledDate = Event.ScheduledDate.Add(Event.ScheduledTime);

                var r = UBL.Update(Event, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Event.ActionType = "UPDATE";
                    Event.MinisterList = MBL.List(true);
                    return View(Event);
                }
            }
            else
            {
                Event.MinisterList = MBL.List(true);
                return View(Event);
            }
        }

        public ActionResult Disable(int id)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            UpcommingEvents Event = UBL.Details(id);
            Event.ActionType = "DISABLE";
            string InsertUser = User.Identity.GetUserName();

            var r = UBL.Update(Event, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return this.RedirectToAction("Index", "UpcommingEvents");
            }
        }
    }
}