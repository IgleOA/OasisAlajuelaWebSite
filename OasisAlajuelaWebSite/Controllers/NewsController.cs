using System;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using System.IO;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Configuration;
using shortid;

namespace OasisAlajuelaWebSite.Controllers
{
    public class NewsController : Controller
    {
        private NewsBL NBL = new NewsBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();
        private HelpersBL HBL = new HelpersBL();

        public ActionResult Index()
        {
            var list = NBL.List(true);
            if (Request.IsAuthenticated)
            {
                USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
                if (validation.ReadRight == false)
                {
                    ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Users user = USBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                    if (user.RoleName.Contains("Admin"))
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
        public ActionResult History(string currentFilter, string searchString, int? page)
        {
            var list = from n in NBL.List(false)
                       select n;
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.Title.Contains(searchString) || b.Description.Contains(searchString));
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
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
                USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(MS);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(News MS)
        {
            String FileExt = Path.GetExtension(MS.UploadFile.FileName).ToUpper();
            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
            {
                string GUID = "IMG_New_" + ShortId.Generate(true, false, 12) + FileExt;

                string ServerPath = Path.Combine(Server.MapPath("~/Files/Images"), GUID);

                //MS.UploadFile.SaveAs(ServerPath);
                HBL.ResizeAndSaveAzure(850, MS.UploadFile, ServerPath);

                MS.BannerPath = ConfigurationManager.AppSettings["AzureStorage"].ToString() + "images/" + GUID;

                MS.InsertDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"]));

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
            else
            {
                this.ModelState.AddModelError(String.Empty, "La imagen seleccionada es de un formato invalido o no aceptado.");
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
                InsertDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])),
                ActionType = "CHGST"
            };

            var r = NBL.Update(New, InsertUser);

            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));

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
        public ActionResult ChangeShow(int id)
        {

            string InsertUser = User.Identity.GetUserName();

            News New = new News()
            {
                NewID = id,
                InsertDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])),
                ActionType = "CHGVIS"
            };

            var r = NBL.Update(New, InsertUser);
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));

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
                USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(Event);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News MS)
        {
            string InsertUser = User.Identity.GetUserName();

            if (MS.UploadFile != null)
            {
                String FileExt = Path.GetExtension(MS.UploadFile.FileName).ToUpper();

                if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                {
                    string GUID = "IMG_New_" + ShortId.Generate(true, false, 12) + FileExt;

                    string ServerPath = Path.Combine(Server.MapPath("~/Files/Images"), GUID);

                    //MS.UploadFile.SaveAs(ServerPath);
                    HBL.ResizeAndSaveAzure(850, MS.UploadFile, ServerPath);

                    MS.BannerPath = ConfigurationManager.AppSettings["AzureStorage"].ToString() + "images/" + GUID;

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