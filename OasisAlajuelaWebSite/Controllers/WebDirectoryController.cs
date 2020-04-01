using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class WebDirectoryController : Controller
    {

        private WebDirectoryBL WBL = new WebDirectoryBL();
        private RightsBL RBL = new RightsBL();

        public ActionResult Index()
        {
            var validation = RBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                ViewBag.WriteRight = validation.WriteRight;

                var list = WBL.List();
                return View(list.ToList());
            }
            
        }

        public ActionResult AddNew()
        {
            var validation = RBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                WebDirectory WD = new WebDirectory();
                return View(WD);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(WebDirectory WD)
        {
            if(ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();
                var r = WBL.AddNew(WD, InsertUser);
                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    WD.ActionType = "CREATE";
                    return View(WD);
                }
            }
            else
            {
                return View(WD);
            }

        }
    }
}