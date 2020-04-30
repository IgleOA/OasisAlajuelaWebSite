using ET;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class MinistersController : Controller
    {
        private MinistersBL MBL = new MinistersBL();
        private UsersBL UBL = new UsersBL();
        public ActionResult AddNew()
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
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
    }
}