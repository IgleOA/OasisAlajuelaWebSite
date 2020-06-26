using System;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using OasisAlajuelaWebSite.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Configuration;
using shortid;

namespace OasisAlajuelaWebSite.Controllers
{ 
    [AllowSameSite]
    public class MinistriesController : Controller
    {
        private MinistriesBL MBL = new MinistriesBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL UBL = new UsersBL();
        // GET: Ministries
        public ActionResult Index()
        {
            var list = MBL.List();

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
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Ministries Ministry = new Ministries();
                UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(Ministry);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Ministries Min)
        {
            String FileExt = Path.GetExtension(Min.UploadFile.FileName).ToUpper();

            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
            {
                string GUID = "IMG_Ministry_" + ShortId.Generate(true, false, 12) + FileExt;

                string ServerPath = Path.Combine(Server.MapPath("~/Files/Images"), GUID);

                Min.UploadFile.SaveAs(ServerPath);

                Min.ImagePath = "/Files/Images/" + GUID;

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
                this.ModelState.AddModelError(String.Empty, "La imagen selecciona es de un formato invalido o no aceptado.");
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
                Ministries Event = MBL.Details(id);
                UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(Event);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ministries Min)
        {
            if (Min.UploadFile == null)
            {
                var r = MBL.Update(Min, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Ministries Event = MBL.Details(Min.MinistryID);
                    Event.ActionType = "UPDATE";
                    return View(Event);
                }                
            }
            else
            {
                String FileExt = Path.GetExtension(Min.UploadFile.FileName).ToUpper();

                if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                {
                    string GUID = "IMG_Ministry_" + ShortId.Generate(true, false, 12) + FileExt;

                    string ServerPath = Path.Combine(Server.MapPath("~/Files/Images"), GUID);

                    Min.UploadFile.SaveAs(ServerPath);

                    Min.ImagePath = "/Files/Images/" + GUID;

                    var r = MBL.Update(Min, User.Identity.GetUserName());

                    if (!r)
                    {
                        ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        Ministries Event = MBL.Details(Min.MinistryID);
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

        public ActionResult Disable(int id)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Ministries Event = MBL.Details(id);
                Event.ActionType = "DISABLE";
                string InsertUser = User.Identity.GetUserName();

                var r = MBL.Update(Event, InsertUser);
                UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
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
}