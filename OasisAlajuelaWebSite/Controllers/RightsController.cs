﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class RightsController : Controller
    {
        private RightsBL RBL = new RightsBL();
        private RolesBL RRBL = new RolesBL();

        public ActionResult Index(int id)
        {
            var data = RBL.List(id);

            var role = (from r in RRBL.List()
                        where r.RoleID == id
                        select r.RoleName).FirstOrDefault().ToString();

            ViewBag.RoleName = role;

            return View(data.ToList());
        }

        [HttpPost]
        public ActionResult RemoveAcess(Rights id)
        {
            
            if(id.ChangeType == "Read")
            {
                id.ReadRight = false;
                id.WriteRight = false;
            }
            else
            {
                id.WriteRight = false;
            }

            string InsertUser = User.Identity.GetUserName();

            var r = RBL.Update(id, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return RedirectToAction("Index", new { id = id.RoleID });
            }
        }

        [HttpPost]
        public ActionResult GrantAcess(Rights id)
        {

            if (id.ChangeType == "Read")
            {
                id.ReadRight = true;
            }
            else
            {
                id.ReadRight = true;
                id.WriteRight = true;
            }

            string InsertUser = User.Identity.GetUserName();

            var r = RBL.Update(id, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return RedirectToAction("Index", new { id = id.RoleID });
            }
        }
    }
}