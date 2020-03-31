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
    public class MinistriesController : Controller
    {
        private MinistriesBL MBL = new MinistriesBL();
        // GET: Ministries
        public ActionResult Index(string id)
        {
            var list = MBL.List();

            if (id == "Admin")
            {
                if (Request.IsAuthenticated)
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                    ViewBag.Page = "Admin";
                    return View(list.ToList());
                }
                else
                {
                    return this.RedirectToAction("Login", "Account");
                }
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                ViewBag.Page = "Public";
                return View(list.ToList());
            }
        }

        public ActionResult _FooterMinistries()
        {
            var list = MBL.List();

            return View(list.ToList());
        }

        [Authorize]
        public ActionResult AddNew()
        {
            Ministries Ministry = new Ministries();

            return View(Ministry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Ministries Min)
        {
            if(ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                var r = MBL.AddNew(Min, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Min.ActionType = "CREATE";
                    return View(Min);
                }
            }
            else
            {
                return View(Min);
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Ministries Event = MBL.Details(id);
            
            return View(Event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ministries Event)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                var r = MBL.Update(Event, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Event.ActionType = "UPDATE";
                    return View(Event);
                }
            }
            else
            {
                return View(Event);
            }
        }

        public ActionResult Disable(int id)
        {
            Ministries Event = MBL.Details(id);
            Event.ActionType = "DISABLE";
            string InsertUser = User.Identity.GetUserName();

            var r = MBL.Update(Event, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return this.RedirectToAction("Index", "Ministries", new { id = "Admin" });
            }
        }
    }
}