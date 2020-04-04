using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using OasisAlajuelaWebSite.Models;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private RightsBL RRBL = new RightsBL();
        private ServicesBL SBL = new ServicesBL();
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
                var list = SBL.List(false);
                ViewBag.Write = validation.WriteRight;
                return View(list.ToList());
            }
        }

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
                Services SVC = new Services();
                return View(SVC);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Services SVC)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                var r = SBL.AddNew(SVC, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    SVC.ActionType = "CREATE";
                    return View(SVC);
                }
            }
            else
            {
                return View(SVC);
            }
        }

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
                var data = from s in SBL.List(false)
                           where s.ServiceID == id
                           select s;

                Services SVC = data.FirstOrDefault();

                return View(SVC);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Services SVC)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                var r = SBL.Update(SVC, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    SVC.ActionType = "UPDATE";
                    return View(SVC);
                }
            }
            else
            {
                return View(SVC);
            }
        }

        public ActionResult ChangeStatus(int id)
        {
            string InsertUser = User.Identity.GetUserName();

            Services SVC = new Services()
            {
                ServiceID = id
            };

            var r = SBL.Update(SVC, InsertUser);

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
    }
}