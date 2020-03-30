using System;
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

        public ActionResult Index(string id)
        {
            List<UpcommingEvents> list = new List<UpcommingEvents>();

            ViewBag.Type = id;

            if (id == "Public")
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                list = UBL.List(DateTime.Now, true, true);                
                return View(list.ToList());
            }
            else
            {
                if (Request.IsAuthenticated)
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                    list = UBL.List(DateTime.Now, true, true);
                    return View(list.ToList());
                }
                else
                {
                    return this.RedirectToAction("Login", "Account");
                }
            }
        }

        [Authorize]
        public ActionResult AddNew()
        {
            UpcommingEvents Event = new UpcommingEvents()
            {
                MinisterList = MBL.List(true)
            };

            return View(Event);
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
            UpcommingEvents Event = UBL.Details(id);

            Event.MinisterList = MBL.List(true);            

            return View(Event);
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