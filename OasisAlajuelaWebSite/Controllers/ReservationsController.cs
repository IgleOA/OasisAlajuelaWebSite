using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OasisAlajuelaWebSite.Models;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Text;
using System.Net.Mail;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private AuditoriumLayoutBL ABL = new AuditoriumLayoutBL();
        private WorshipsBL WBL = new WorshipsBL();
        private UsersBL UBL = new UsersBL();
        private ReservationsBL RBL = new ReservationsBL();
        private RightsBL RRBL = new RightsBL();

        // GET: Reservations
        public ActionResult Index(int id)
        {        
           UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                var u = from us in UBL.List()
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

                Worships Model = WBL.Details(id);

                Model.Layout = ABL.Layout(id);

                return View(Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(Worships Model)
        {
            Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

            Reservations Reservation = new Reservations()
            {
                GUID = Guid.NewGuid().ToString().ToUpper(),
                WorshipID = Model.WorshipID,
                BookedBy = user.UserID,
                BookedFor = (string.IsNullOrEmpty(Model.ReservedFor)) ? user.FullName : Model.ReservedFor,
                SeatsReserved = Model.SeatsReserved
            };

            Reservation.Details = RBL.AddReservation(Reservation, User.Identity.GetUserName());
            
            return RedirectToAction("Confirmation","Reservations", new { id = Reservation.GUID });
        }

        

        public ActionResult Confirmation(string id)
        {

            List<Reservations> data = RBL.Details(id);


            Users user = UBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

            // Email 
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
            // Email

            int roleID = user.RoleID;

            if (roleID == 2 || roleID == 3 || roleID == 4)
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
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
            string InsertUser = User.Identity.GetUserName();
            
            string r = RBL.Remove(id, InsertUser);

            return this.RedirectToAction("Confirmation", new { id = r });
            
        }
        public ActionResult EmailConfirmation(string id)
        {
            var data = RBL.Details(id);

            return View(data.ToList());
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