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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HomePage()
        {
            HomePage HP = HBL.Home();

            return View(HP);
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