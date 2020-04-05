using System.Linq;
using System.Web.Mvc;
using BL;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class SermonsController : Controller
    {
        private YouTubeBL YBL = new YouTubeBL();
        private SermonsBL SBL = new SermonsBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();

        public ActionResult Index(string id)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                //var data = from d in YBL.Youtubelist(50)
                //           orderby d.PublishedAt descending
                //           select d;

                var data = SBL.List();

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
                return View(data.ToList());
            }
        }
    }
}