using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using System.IO;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    public class NewsController : Controller
    {
        private NewsBL NBL = new NewsBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();

        public ActionResult Index()
        {
            var list = NBL.List(true);
            if (Request.IsAuthenticated)
            {
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
                    }
                    else
                    {
                        ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                    }
                    ViewBag.Write = validation.WriteRight;
                    return View(list);
                }
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View(list);
            }
        }

        [Authorize]
        public ActionResult History()
        {
            var list = NBL.List(false);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return View(list);
            }
            
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
                News MS = new News();
                return View(MS);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(News MS)
        {
            String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

            MS.BannerExt = FileExt;

            Stream str = MS.file.InputStream;
            BinaryReader Br = new BinaryReader(str);
            Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

            MS.BannerData = FileDet;
            MS.InsertDate = DateTime.Now;

            string InsertUser = User.Identity.GetUserName();

            var r = NBL.AddNew(MS, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                MS.ActionType = "CREATE";

                return View(MS);
            }            
        }

        [Authorize]
        public ActionResult ChangeStatus(int id)
        {

            string InsertUser = User.Identity.GetUserName();

            News New = new News()
            {
                NewID = id,
                InsertDate = DateTime.Now
            };

            var r = NBL.Update(New, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return this.RedirectToAction("Index");
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                News Event = NBL.Details(id);

                return View(Event);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News MS)
        {
            string InsertUser = User.Identity.GetUserName();

            if (MS.file != null)
            {
                String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

                MS.BannerExt = FileExt;

                if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                {
                    Stream str = MS.file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                    MS.BannerData = FileDet;

                    var r = NBL.Update(MS, InsertUser);

                    if (!r)
                    {
                        ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        MS.ActionType = "UPDATE";
                        return View(MS);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(String.Empty, "Por favor seleccione un banner en formato JPG, JPEG o PNG.");
                    return View(MS);                    
                }
            }
            else
            {
                var r = NBL.Update(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    MS.ActionType = "UPDATE";
                    return View(MS);
                }
            }
        }
    }
}