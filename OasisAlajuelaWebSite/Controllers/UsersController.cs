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
        private UserProfileBL UPBL = new UserProfileBL();

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
                    list = list.Where(b => b.FullName.ToLower().Contains(searchString.ToLower()) | b.UserName.ToLower().Contains(searchString.ToLower()));
                }
                
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult ChangeStatus(int id)
        {
            string InsertUser = User.Identity.GetUserName();

            Users user = new Users()
            {
                UserID = id,
                ActionType = "CHGST"
            };

            var r = UBL.Update(user, InsertUser);

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

        public new ActionResult Profile(int id)
        {
            var r = UPBL.Detail(id);

            if (r.LastActivityDate.ToString().Length == 0)
            {
                r.LastActivityDate = r.CreationDate;
            }

            if (r.UserName == User.Identity.GetUserName())
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                ViewBag.Write = true;
                return View(r);
            }
            else
            {
                var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(),"Index");
                if (validation.ReadRight == false)
                {
                    ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    ViewBag.Write = false;
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                    return View(r);
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateMainInfo(UserProfile UP)
        {
            string insertuser = User.Identity.GetUserName();

            Users user = new Users()
            {
                UserID = UP.UserID,
                FullName = UP.FullName,
                UserName = UP.UserName,
                RoleID = UP.RoleID,
                ActiveFlag = true,
                ActionType = "UPDATE"
            };
            var r = UBL.Update(user, insertuser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return RedirectToAction("Profile", new { id = UP.UserID });
            }
        }

        [HttpPost]
        public ActionResult UpdateContactInfo(UserProfile UP)
        {
            string insertuser = User.Identity.GetUserName();

            UP.ActionType = "CONTACT";
            UP.PhotoData = null;

            var r = UPBL.Update(UP, insertuser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return RedirectToAction("Profile", new { id = UP.UserID });
            }
        }

        [HttpPost]
        public ActionResult UpdateSNInfo(UserProfile UP)
        {
            string insertuser = User.Identity.GetUserName();

            UP.ActionType = "SOCIALNET";
            UP.PhotoData = null;

            var r = UPBL.Update(UP, insertuser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return RedirectToAction("Profile", new { id = UP.UserID });
            }
        }
    }
}