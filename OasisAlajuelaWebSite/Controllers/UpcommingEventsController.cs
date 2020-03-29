using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Index(string Type)
        {
            List<UpcommingEvents> list = new List<UpcommingEvents>();

            if (Type == "Public")
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
                    list = UBL.List(DateTime.Now, true, null);
                    return View(list.ToList());
                }
                else
                {
                    return this.RedirectToAction("Login", "Account");
                }
            }
            return View();
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
    }
}