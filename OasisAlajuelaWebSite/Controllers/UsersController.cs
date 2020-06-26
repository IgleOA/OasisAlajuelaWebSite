using BL;
using ET;
using Microsoft.AspNet.Identity;
using OasisAlajuelaWebSite.Models;
using PagedList;
using shortid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
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
                    list = list.Where(b => b.FullName.ToLower().Contains(searchString.ToLower()) | b.UserName.ToLower().Contains(searchString.ToLower()) | b.Email.ToLower().Contains(searchString.ToLower()));
                }

                ViewBag.UsersCount = list.Count();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult ChangeStatus(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));

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
            foreach (var l in r.GroupList)
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
                var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            string insertuser = User.Identity.GetUserName();

            UP.ActionType = "CONTACT";
            UP.Photo = null;

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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            string insertuser = User.Identity.GetUserName();

            UP.ActionType = "SOCIALNET";
            UP.Photo = null;

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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            string insertuser = User.Identity.GetUserName();

            UP.PhotoExt = Path.GetExtension(UP.file.FileName).ToUpper();

            if (UP.PhotoExt == ".PNG" || UP.PhotoExt == ".JPG" || UP.PhotoExt == ".JPEG")
            {
                UP.ActionType = "PHOTO";

                string GUID = "IMG_Profile_" + ShortId.Generate(true, false, 12) + UP.PhotoExt;

                string ServerPath = Path.Combine(Server.MapPath("~/Files/Images"), GUID);

                UP.file.SaveAs(ServerPath);
                UP.Photo = "/Files/Images/" + GUID;
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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
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
                        FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                        ToEmail = UUser.Email,
                        SubjectEmail = "Oasis Alajuela - Registro satisfactorio",
                        BodyEmail = "Gracias " + UUser.FullName + " por registrarse y por tener el sentir de hacerte parte de esta familia. Dios trae cosas grandes para esta casa y ahora seras parte de ellas. Bendiciones..."
                    };

                    StringBuilder mailBody = new StringBuilder();

                    var strg =  ViewToStringRenderer.RenderViewToString(this.ControllerContext, "~/Views/Users/EmailConfirmation.cshtml", UUser);

                    mailBody.AppendFormat(strg);

                    Email.BodyEmail = mailBody.ToString();

                    MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                    mm.Subject = Email.SubjectEmail;
                    mm.Body = Email.BodyEmail;
                    mm.IsBodyHtml = true;

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

        public void AdminResetPassword(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));

            var r = UBL.AdminResetPassword(id, User.Identity.GetUserName());

            Users user = UBL.Details(id);
            user.Password = "!234s6789";

            #region Email
            Emails Email = new Emails()
            {
                FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                ToEmail = user.Email,
                SubjectEmail = "Su contraseña de OasisAlajuela.com ha sido restablecida"
            };

            StringBuilder mailBody = new StringBuilder();
            var strg = ViewToStringRenderer.RenderViewToString(this.ControllerContext, "~/Views/Users/ResetPasswordConfirmation.cshtml", user);

            mailBody.AppendFormat(strg);


            Email.BodyEmail = mailBody.ToString();

            MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
            mm.Subject = Email.SubjectEmail;
            mm.Body = Email.BodyEmail;
            mm.IsBodyHtml = true;
            mm.BodyEncoding = Encoding.GetEncoding("utf-8");

            SmtpClient smtp = new SmtpClient();
            smtp.Send(mm); 
            #endregion
        }

        public ActionResult ResetPasswordConfirmation(Users UUser)
        {
            return View(UUser);
        }

        public ActionResult EmailConfirmation(Users UUser)
        {
            return View(UUser);
        }

        public static class ViewToStringRenderer
        {
            public static string RenderViewToString<TModel>(ControllerContext controllerContext, string viewName, TModel model)
            {
                ViewEngineResult viewEngineResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
                if (viewEngineResult.View == null)
                {
                    throw new Exception("Could not find the View file. Searched locations:\r\n" + viewEngineResult.SearchedLocations);
                }
                else
                {
                    IView view = viewEngineResult.View;

                    using (var stringWriter = new StringWriter())
                    {
                        var viewContext = new ViewContext(controllerContext, view, new ViewDataDictionary<TModel>(model), new TempDataDictionary(), stringWriter);
                        view.Render(viewContext, stringWriter);

                        return stringWriter.ToString();
                    }
                }
            }
        }

        public ActionResult AddGroup(int id)
        {
            MultiSelectDropDownViewModel model = new MultiSelectDropDownViewModel
            {
                UserID = id,
                SelectedMultiGroupId = new List<int>(),
                SelectedGroupLst = new List<Groups>()
            };

            try
            {
                this.ViewBag.GroupList = this.GetGroupList(id);
                this.ViewBag.UserID = id;
                Users user = UBL.Details(id);
                this.ViewBag.UserFullName = user.FullName;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGroup(MultiSelectDropDownViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model.SelectedMultiGroupId)
                    {
                        UsersGroups UG = new UsersGroups
                        {
                            UserID = model.UserID,
                            GroupID = item
                        };
                        var r = GBL.AddUserGroup(UG, User.Identity.GetUserName());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            this.ViewBag.GroupList = this.GetGroupList(model.UserID);
            this.ViewBag.UserID = model.UserID;
            Users user = UBL.Details(model.UserID);
            this.ViewBag.UserFullName = user.FullName;
            model.ActionType = "CREATE";
            return this.View(model);
        }

        public ActionResult RemoveUG(int UserID, int GroupID)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            string InsertUser = User.Identity.GetUserName();

            UsersGroups UG = new UsersGroups()
            {
                UserID = UserID,
                GroupID = GroupID
            };

            var r = GBL.RemoveUG(UG, InsertUser);

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

        private IEnumerable<SelectListItem> GetGroupList(int id)
        {
            SelectList lstobj = null;

            try
            {
                var data = from r in GBL.List()
                           where !(from d in GBL.ListbyUser(id)
                                   select d.GroupID).Contains(r.GroupID)
                           select r;

                //var list = this.GBL.List().Select(p => new SelectListItem { Value = p.GroupID.ToString(), Text = p.GroupName });
                var list = data.Select(p => new SelectListItem { Value = p.GroupID.ToString(), Text = p.GroupName });

                lstobj = new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstobj;
        }
    }
}