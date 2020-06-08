using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using OasisAlajuelaWebSite.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Configuration;

namespace OasisAlajuelaWebSite.Controllers
{
    [AllowSameSite]
    public class LeadershipController : Controller
    {
        private LeadershipBL LBL = new LeadershipBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL UBL = new UsersBL();

        public ActionResult Index()
        {
            var list = LBL.List();

            if (Request.IsAuthenticated)
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
                    Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                    if (user.RoleName.Contains("Admin"))
                    {
                        ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                    }
                    else
                    {
                        ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                    }
                    ViewBag.Write = validation.WriteRight;
                    return View(list.ToList());
                }
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                ViewBag.Page = "Public";
                return View(list.ToList());
            };
        }

        #region DetailsPage
        public ActionResult Gerardo_Montero()
        {
            if (Request.IsAuthenticated)
            {
                Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }
                return View();
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View();
            }
        }

        public ActionResult Alicia_Guillen()
        {
            if (Request.IsAuthenticated)
            {
                Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }
                return View();
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View();
            }
        }
        #endregion

        public ActionResult _FooterLeadership()
        {
            var list = LBL.List().Where(x => x.ActionLink.Length > 0);
            return View(list.ToList());
        }
               

        [Authorize]
        public ActionResult AddNew()
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Leadership Leader = new Leadership();
                UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(Leader);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Leadership Min)
        {
            if (ModelState.IsValid)
            {
                String FileExt = Path.GetExtension(Min.file.FileName).ToUpper();

                Min.ImageExt = FileExt;

                if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                {
                    Stream str = Min.file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                    Min.Image = FileDet;

                    string InsertUser = User.Identity.GetUserName();

                    var r = LBL.AddNew(Min, InsertUser);

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
                    this.ModelState.AddModelError(String.Empty, "La imagen selecciona es de un formato invalido o no aceptado.");
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
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Leadership Leader = LBL.Details(id);
                UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(Leader);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Leadership Min)
        {
            if (Min.file == null)
            {
                var r = LBL.Update(Min, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Leadership Event = LBL.Details(Min.LeaderID);
                    Event.ActionType = "UPDATE";
                    return View(Event);
                }
            }
            else
            {
                String FileExt = Path.GetExtension(Min.file.FileName).ToUpper();

                Min.ImageExt = FileExt;

                if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                {
                    Stream str = Min.file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                    Min.Image = FileDet;

                    var r = LBL.Update(Min, User.Identity.GetUserName());

                    if (!r)
                    {
                        ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        Leadership Event = LBL.Details(Min.LeaderID);
                        Event.ActionType = "UPDATE";
                        return View(Event);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(String.Empty, "La imagen selecciona es de un formato invalido o no aceptado.");
                    return View(Min);
                }
            }
        }
    }
}