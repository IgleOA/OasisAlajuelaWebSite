using ET;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net.Mail;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class EnrollmentsController : Controller
    {
        private EnrollmentsBL EBL = new EnrollmentsBL();
        private GroupsBL GBL = new GroupsBL();
        private UsersBL UBL = new UsersBL();
        private RightsBL RRBL = new RightsBL();
        private UserProfileBL UPBL = new UserProfileBL();

        // GET: Enrollments
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

                var list = from d in EBL.List(true)
                           select d;

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.GroupName.ToLower().Contains(searchString.ToLower()));
                }

                ViewBag.UsersCount = list.Count();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Add()
        {
            Enrollments Detail = new Enrollments()
            {
                GroupList = GBL.List(),
                OpenRegister = DateTime.Today,
                CloseRegister = DateTime.Today.AddDays(7)
            };

            return View(Detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Enrollments Detail)
        {
            if (ModelState.IsValid)
            {
                var r = EBL.Add(Detail, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Detail.ActionType = "CREATE";
                    Detail.GroupList = GBL.List();
                    return View(Detail);
                }
            }
            else
            {
                Detail.GroupList = GBL.List();
                return View(Detail);
            }
        }

        public ActionResult EnrollUser(int id = 0)
        {
            AdminEnroller Detail = new AdminEnroller()
            {
                UserList = UBL.List().Where(x => x.ActiveFlag == true).ToList()
            };
            if(id > 0)
            {
                Detail.Courses = EBL.List(true).Where(x => x.EnrollmentID == id).ToList();
            }
            else
            {
                Detail.Courses = EBL.List(false);
            }

            return View(Detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnrollUser(AdminEnroller Detail)
        {
            if (ModelState.IsValid)
            {
                Users userinfo = UBL.Details(Detail.UserID);
                UserProfile UserProfile = UPBL.Detail(Detail.UserID);

                EnrolledUsers NewUser = new EnrolledUsers()
                {
                    UserID = Detail.UserID,
                    FullName = userinfo.FullName,
                    PhoneNumber = UserProfile.Mobile,
                    EnrollmentID = Detail.EnrollmentID
                };

                var r = EBL.AddUser(NewUser, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Detail.ActionType = "CREATE";
                    Detail.UserList = UBL.List().Where(x => x.ActiveFlag == true).ToList();
                    Detail.Courses = EBL.List(true).Where(x => x.EnrollmentID == Detail.EnrollmentID).ToList();
                    return View(Detail);
                }
            }
            else
            {
                Detail.UserList = UBL.List().Where(x => x.ActiveFlag == true).ToList();
                Detail.Courses = EBL.List(true).Where(x => x.EnrollmentID == Detail.EnrollmentID).ToList();
                return View(Detail);
            }
        }

        public void RemoveEnrollment(int id)
        {
            EBL.RemoveEnrollment(id, User.Identity.GetUserName());            
        }

        public void RemoveUser(int id)
        {
            EBL.RemoveUser(id);
        }
        public void ApproveEnrollment(int id)
        {
            EBL.ApproveEnrollment(id);
        }

        public ActionResult Enroller()
        {
            Users UserInfo = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();
            UserProfile UserProfile = UPBL.Detail(UserInfo.UserID);

            EnrolledUsers Enroller = new EnrolledUsers()
            {
                UserID = UserInfo.UserID,
                FullName = UserInfo.FullName,
                PhoneNumber = UserProfile.Mobile,
                Courses = EBL.List(false)
            };
            if (UserInfo.RoleName.Contains("Admin"))
            {
                ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
            }
            return View(Enroller);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroller(EnrolledUsers Detail)
        {
            if (ModelState.IsValid)
            {
                var r = EBL.AddUser(Detail, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Users UserInfo = UBL.List().Where(x => x.UserID == Detail.UserID).FirstOrDefault();
                    Enrollments Enroll = EBL.List(true).Where(x => x.EnrollmentID == Detail.EnrollmentID).FirstOrDefault();
                    EnrollerInfo Info = new EnrollerInfo()
                    {
                        GroupName = Enroll.GroupName,
                        FullName = UserInfo.FullName
                    };

                    #region Email
                    Emails Email = new Emails()
                    {
                        FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                        ToEmail = UserInfo.Email,
                        SubjectEmail = "Oasis Alajuela - Inscripción"
                    };

                    StringBuilder mailBody = new StringBuilder();

                    var strg = ViewToStringRenderer.RenderViewToString(this.ControllerContext, "~/Views/Enrollments/EmailConfirmation.cshtml", Info);

                    mailBody.AppendFormat(strg);

                    Email.BodyEmail = mailBody.ToString();

                    MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                    mm.Subject = Email.SubjectEmail;
                    mm.Body = Email.BodyEmail;
                    mm.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mm);
                    #endregion

                    return this.RedirectToAction("EnrollerConfirmation", "Enrollments", new { id = Info.GroupName });
                }
            }
            else
            {
                Detail.Courses = EBL.List(true).Where(x => x.EnrollmentID == Detail.EnrollmentID).ToList();
                return View(Detail);
            }
        }

        public ActionResult EnrollerConfirmation(string id)
        {
            Users UserInfo = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

            if (UserInfo.RoleName.Contains("Admin"))
            {
                ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
            }
            ViewBag.GroupName = id;
            return View();

        }

        public ActionResult EmailConfirmation(EnrollerInfo id)
        {            
            return View(id);
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
    }
}