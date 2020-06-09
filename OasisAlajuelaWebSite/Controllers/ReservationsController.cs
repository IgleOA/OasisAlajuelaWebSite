using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Text;
using System.Net.Mail;
using shortid;
using PagedList;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private AuditoriumLayoutBL ABL = new AuditoriumLayoutBL();
        private ReservationEventDetailBL WBL = new ReservationEventDetailBL();
        private UsersBL UBL = new UsersBL();
        private ReservationsBL RBL = new ReservationsBL();
        private RightsBL RRBL = new RightsBL();
        private UserProfileBL UPBL = new UserProfileBL();

        // GET: Reservations
        public ActionResult Index()
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));

            Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

            List<ReservationLevel1> Reservations = RBL.ReservationsMainInfo(0, user.UserID);
            if (user.RoleName.Contains("Admin"))
            {
                ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";                
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
            }

            return View(Reservations);
        }

        public ActionResult Master(string currentFilter, string searchString, int? page)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));

            var Reservations = from r in RBL.ReservationsMaster()
                               select r;

            //return View(Reservations);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                Reservations = Reservations.Where(b => b.BookedByName.Contains(searchString) || b.Title.Contains(searchString) || b.GUID.Contains(searchString) || b.BookedFor.Contains(searchString));
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Reservations.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CheckOut(int id)
        {        
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                ////if (user.RoleName.Contains("Admin"))
                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }
                ViewBag.Write = validation.WriteRight;

                ReservationEventDetail Model = WBL.Details(id,user.UserID);

                Model.Layout = ABL.Layout(id);

                return View(Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(ReservationEventDetail Model)
        {
            if (ModelState.IsValid)
            {
                Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                Reservations Reservation = new Reservations()
                {
                    //GUID = Guid.NewGuid().ToString().ToUpper(),
                    //GUID = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                    GUID = ShortId.Generate(true, false, 12),
                    EventID = Model.EventID,
                    BookedBy = user.UserID,
                    BookedFor = (string.IsNullOrEmpty(Model.ReservedFor)) ? user.FullName : Model.ReservedFor,
                    SeatsReserved = Model.SeatsReserved
                };

                Reservation.Details = RBL.AddReservation(Reservation, User.Identity.GetUserName());

                List<Reservations> Reservations = RBL.ReservationsFullInfo(Model.EventID, user.UserID);

                if(Reservations.Count >= 20 && !user.RoleName.Contains("Admin"))
                {
                    #region Email
                    Emails Email = new Emails()
                    {
                        FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                        ToEmail = ConfigurationManager.AppSettings["ValidationsEmail"].ToString(),
                        SubjectEmail = "Oasis Alajuela - Confirmación de Reservas - " + user.FullName
                    };

                    StringBuilder mailBody = new StringBuilder();

                    UserProfile UserModel = UPBL.Detail(user.UserID);

                    UserModel.ReservationsList = Reservations;

                    var strg = ViewToStringRenderer.RenderViewToString(this.ControllerContext, "~/Views/Reservations/ValidationEmail.cshtml", UserModel);

                    mailBody.AppendFormat(strg);

                    Email.BodyEmail = mailBody.ToString();

                    MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                    mm.Subject = Email.SubjectEmail;
                    mm.Body = Email.BodyEmail;
                    mm.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mm);
                    #endregion
                }

                return RedirectToAction("Confirmation", "Reservations", new { id = Reservation.GUID });
            }
            else
            {
                Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }
                //ViewBag.Write = validation.WriteRight;
                ReservationEventDetail Model2 = WBL.Details(Model.EventID, user.UserID);

                Model.MaxToReserve = Model2.MaxToReserve;

                Model.Layout = ABL.Layout(Model.EventID);
                return View(Model);
            }
        }

        public ActionResult _PreviousReservations(int id)
        {
            Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

            List<ReservationLevel1> Reservations = RBL.ReservationsMainInfo(id, user.UserID);
            
            return View(Reservations);
        }

        public ActionResult Confirmation(string id)
        {

            List<Reservations> data = RBL.Details(id);

            Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();


            #region Email
            Emails Email = new Emails()
            {
                FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                ToEmail = user.Email,
                SubjectEmail = "Oasis Alajuela - Confirmación de Reserva"
            };

            StringBuilder mailBody = new StringBuilder();

            var strg = ViewToStringRenderer.RenderViewToString(this.ControllerContext, "~/Views/Reservations/EmailConfirmation.cshtml", data.ToList());

            mailBody.AppendFormat(strg);

            Email.BodyEmail = mailBody.ToString();

            MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
            mm.Subject = Email.SubjectEmail;
            mm.Body = Email.BodyEmail;
            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Send(mm); 
            #endregion


            int roleID = user.RoleID;

            if (user.RoleName.Contains("Admin"))
            {
                ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
            }

            return View(data.ToList());
        }

        public ActionResult Remove(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            string InsertUser = User.Identity.GetUserName();
            
            string r = RBL.Remove(id, InsertUser);

            return JavaScript("<script>alert(\"Hecho!!!\")</script>");
            
        }

        public ActionResult RemoveGUID(string id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"])));
            string InsertUser = User.Identity.GetUserName();

            var r = RBL.RemoveGUID(id, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return JavaScript("<script>alert(\"Hecho!!!\")</script>");
            }

        }

        public ActionResult EmailConfirmation(string id)
        {
            var data = RBL.Details(id);

            return View(data.ToList());
        }

        public ActionResult ValidationEmail(UserProfile id)
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