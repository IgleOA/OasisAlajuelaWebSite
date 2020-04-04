using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        private HomeBL HBL = new HomeBL();
        private BannersBL BBL = new BannersBL();
        private RightsBL RRBL = new RightsBL();
        public ActionResult Index()
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return View();
            }
        }

        public ActionResult HomePage()
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "HomePage");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                HomePage HP = HBL.Home();
                return View(HP);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HomePage(HomePage HP)
        {
            string InsertUser = User.Identity.GetUserName();

            var r = HBL.AddHomePage(HP, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado en su solicitud, por favor intente nuevamente.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                HP.ActionType = "CREATE";
                return View(HP);
            }
        }

        
    }
}