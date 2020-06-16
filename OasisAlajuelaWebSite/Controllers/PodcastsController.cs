using BL;
using ET;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace OasisAlajuelaWebSite.Controllers
{
    public class PodcastsController : Controller
    {
        private PodcastsBL PBL = new PodcastsBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();
        private MinistersBL MBL = new MinistersBL();

        [Authorize]
        public ActionResult Index()
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
                var list = PBL.List();
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
                Podcasts MS = new Podcasts()
                {
                    Ministerlist = MBL.List(true)
                };

                USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(MS);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Podcasts MS)
        {
            String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

            MS.BannerExt = FileExt;

            Stream str = MS.file.InputStream;
            BinaryReader Br = new BinaryReader(str);
            Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

            MS.BannerData = FileDet;
            MS.InsertDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"]));

            string InsertUser = User.Identity.GetUserName();

            var r = PBL.AddNew(MS, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                MS.ActionType = "CREATE";
                MS.Ministerlist = MBL.List(true);
                return View(MS);
            }
        }

        [Authorize]
        public void ChangeStatus(int id)
        {
            Podcasts New = new Podcasts()
            {
                PodcastID = id,
                InsertDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])),
                ActionType = "CHGST"
            };

            PBL.Update(New, User.Identity.GetUserName());
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));           
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
                Podcasts Event = PBL.Details(id);
                Event.Ministerlist = MBL.List(true);
                USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
                return View(Event);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Podcasts MS)
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

                    var r = PBL.Update(MS, InsertUser);

                    if (!r)
                    {
                        ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        MS.ActionType = "UPDATE";
                        MS.Ministerlist = MBL.List(true);
                        return View(MS);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(String.Empty, "Por favor seleccione un banner en formato JPG, JPEG o PNG.");
                    MS.Ministerlist = MBL.List(true);
                    return View(MS);
                }
            }
            else
            {
                var r = PBL.Update(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    MS.ActionType = "UPDATE";
                    MS.Ministerlist = MBL.List(true);
                    return View(MS);
                }
            }
        }

        public ActionResult _Podcast()
        {
            var list = PBL.List().Take(5);
            return View(list);
        }
    }
}