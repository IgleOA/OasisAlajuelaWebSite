using System;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Linq;
using PagedList;
using System.IO;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private UsersBL UBL = new UsersBL();
        private RightsBL RRBL = new RightsBL();

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
                ViewBag.WriteRight = validation.WriteRight;
                
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var list = from b in UBL.List()
                              select b;

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.FullName.Contains(searchString) | b.UserName.Contains(searchString));
                }
                
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
        }


    }
}