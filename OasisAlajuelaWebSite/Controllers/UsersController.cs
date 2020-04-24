using System;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Linq;
using PagedList;
using System.IO;
using System.Text;
using System.Net.Mail;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private UsersBL UBL = new UsersBL();
        private RightsBL RRBL = new RightsBL();
        private UserProfileBL UPBL = new UserProfileBL();
        private RolesBL RBL = new RolesBL();
        private GroupsBL GBL = new GroupsBL();

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
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

        public new ActionResult Profile(int id = 0)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);

            if (id == 0)
            {
                var user = from d in UBL.List()
                           where d.UserName == User.Identity.GetUserName()
                           select d;
                id = user.FirstOrDefault().UserID;
            }
            UserProfile r = UPBL.Detail(id);

            r.GroupList = GBL.ListbyUser(id);

            string groups = null;
            foreach(var l in r.GroupList)
            {
                groups += l.GroupName + ",";
            }

            ViewBag.Groups = groups;

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
        public ActionResult UpdateContactInfo(UserProfile UP)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
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

        [HttpPost]
        public ActionResult UpdatePhoto(UserProfile UP)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            string insertuser = User.Identity.GetUserName();

            UP.PhotoExt = Path.GetExtension(UP.file.FileName).ToUpper();

            if (UP.PhotoExt == ".PNG" || UP.PhotoExt == ".JPG" || UP.PhotoExt == ".JPEG")
            {
                UP.ActionType = "PHOTO";

                Stream str = UP.file.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                UP.PhotoData = FileDet;

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
            else
            {
                ViewBag.Mensaje = "El formato de foto seleccionado no es soportado.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult Edit(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                var u = UBL.Details(id);
                u.RolesList = RBL.List();
                return View(u);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users UUser)
        {
            string InsertUser = User.Identity.GetUserName();

            UUser.ActionType = "UPDATE";

            var r = UBL.Update(UUser, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                UUser.RolesList = RBL.List();
                return View(UUser);
            }
        }

        public ActionResult AddNew()
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Users u = new Users();
                u.RolesList = RBL.List();
                return View(u);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Users UUser)
        {
            bool UserNameValidation = UBL.CheckAvailability(UUser.UserName);
            bool EmailValidation = UBL.CheckAvailability(UUser.Email);

            if (UserNameValidation == true && EmailValidation == true)
            {
                UUser.Password = "Wxyz1234";
                var r = UBL.AddUser(UUser, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Emails Email = new Emails()
                    {
                        FromEmail = "johmstone@gmail.com",
                        ToEmail = UUser.Email,
                        SubjectEmail = "Oasis Alajuela - Registro satisfactorio",
                        BodyEmail = "Gracias " + UUser.FullName + " por registrarse y por tener el sentir de hacerte parte de esta familia. Dios trae cosas grandes para esta casa y ahora seras parte de ellas. Bendiciones..."
                    };

                    StringBuilder mailBody = new StringBuilder();

                    mailBody.AppendFormat("<h1>Oasis Alajuela</h1>");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Gracias {0} por registrarse y por tener el sentir de hacerte parte de esta familia. Dios trae cosas grandes para esta casa y ahora seras parte de ellas.</p>", UUser.FullName);
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Desde ya puedes ver el contenido completo de nuestro website http://igleoa.azurewebsites.net/ </p>");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Usuario: {0}</p>", UUser.UserName);
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Contraseña: {0}</p>", UUser.Password);
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<h3>Bendiciones....</h3>");

                    Email.BodyEmail = mailBody.ToString();

                    MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                    mm.Subject = Email.SubjectEmail;
                    mm.Body = Email.BodyEmail;
                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mm);

                    UUser.ActionType = "CREATE";
                    UUser.RolesList = RBL.List();
                    return View(UUser);
                }
            }
            else
            {
                if (UserNameValidation == false && EmailValidation == false)
                {
                    this.ModelState.AddModelError(String.Empty, "Este nombre de usuario y este correo electrónico ya se encuentran registrados, por favor intente con otro correo y otro nombre de usuario.");
                }
                else
                {
                    if (UserNameValidation == false)
                    {
                        this.ModelState.AddModelError(String.Empty, "Este nombre de usuario ya se encuentra registrado, por favor intente con otro nombre de usuario.");
                    }
                    else
                    {
                        this.ModelState.AddModelError(String.Empty, "Este correo electrónico ya se encuentra registrado, por favor intente con otro email.");
                    }
                }
            }

            UUser.RolesList = RBL.List();
            return View(UUser);
        }
    }
}