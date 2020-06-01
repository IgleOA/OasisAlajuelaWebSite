using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using PagedList;
using RotativaHQ .MVC5;

namespace OasisAlajuelaWebSite.Controllers
{
    public class PrayersController : Controller
    {
        private PrayersBL PBL = new PrayersBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL UsBL = new UsersBL();

        [Authorize]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
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

                List<Prayers> prayers = PBL.List(false);

                if (!String.IsNullOrEmpty(searchString))
                {
                    prayers = prayers.Where(b => b.Requester.Contains(searchString) || b.Reason.Contains(searchString) || b.Email.Contains(searchString) || b.PhoneNumber.Contains(searchString)).ToList();
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(prayers.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult PrintVersion(bool id)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                List<Prayers> prayers = PBL.List(id);

                return View(prayers);
            }
        }

        [Authorize]
        public ActionResult History(string currentFilter, string searchString, int? page)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
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

                List<Prayers> prayers = PBL.List(true);

                if (!String.IsNullOrEmpty(searchString))
                {
                    prayers = prayers.Where(b => b.Requester.Contains(searchString) || b.Reason.Contains(searchString) || b.Email.Contains(searchString) || b.PhoneNumber.Contains(searchString)).ToList();
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(prayers.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult New()
        {
            Prayers Prayer = new Prayers();

            if (Request.IsAuthenticated)
            {
                var user = UsBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                Prayer.Requester = user.FullName;
                Prayer.Email = user.Email;

                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }

                return View(Prayer);
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View(Prayer);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Prayers Detail)
        {
            if (ModelState.IsValid)
            {
                Geolocation location = GetGeolocation(Detail.IP);
                Detail.IP = location.Ip;
                Detail.Country = location.Location.Country;
                Detail.Region = location.Location.Region;
                Detail.City = location.Location.City;

                var r = PBL.Add(Detail);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return this.RedirectToAction("Confirmation", "Prayers", new { id = Detail.Requester });
                }
            }
            else
            {
                return View(Detail);
            }
        }

        public ActionResult Confirmation(string id)
        {
            if (Request.IsAuthenticated)
            {
                var user = UsBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();
                                
                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }

                ViewBag.Requester = id;
                return View();
            }
            else
            {
                ViewBag.Requester = id;
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                return View();
            }
        }

        static Geolocation GetGeolocation(string IP)
        {
            string API_URL = ConfigurationManager.AppSettings["GeolocationAPI_URL"].ToString();
            string API_KEY = ConfigurationManager.AppSettings["GeolocationAPI_KEY"].ToString();

            string url = API_URL + $"apiKey={API_KEY}&ipAddress={IP}";
            string resultData = string.Empty;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resultData = reader.ReadToEnd();
            }

            Geolocation location = JsonConvert.DeserializeObject<Geolocation>(resultData);

            return location;
        }

        public ActionResult Export(bool id)
        {
            string filename = "Peticiones_" + DateTime.Today.ToString("dd_MM_yyyy") + ".pdf";

            List<Prayers> prayers = PBL.List(id);

            return new ViewAsPdf("PrintVersion", prayers) { FileName = filename };
        }
    }
}