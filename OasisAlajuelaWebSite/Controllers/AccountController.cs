using System;
using System.Web.Mvc;
using System.Net.Mail;
using ET;
using BL;
using System.Web.Security;
using OasisAlajuelaWebSite.Models;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace OasisAlajuelaWebSite.Controllers
{ 
    [AllowSameSite]
    [Authorize]
    public class AccountController : Controller
    {
        private UsersBL UBL = new UsersBL();
        private static string API_KEY = ConfigurationManager.AppSettings["GeolocationAPI_KEY"].ToString();
        private static string API_URL = ConfigurationManager.AppSettings["GeolocationAPI_URL"].ToString();               

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users User)
        {
            bool UserNameValidation = UBL.CheckAvailability(User.UserName);
            bool EmailValidation = UBL.CheckAvailability(User.Email);

            if (UserNameValidation == true && EmailValidation == true)
            {
                User.RoleID = 1; /*New User*/

                var r = UBL.AddUser(User, User.UserName);

                if(!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Emails Email = new Emails()
                    {
                        FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                        ToEmail = User.Email,
                        SubjectEmail = "Oasis Alajuela - Registro satisfactorio"                        
                    };

                    StringBuilder mailBody = new StringBuilder();
                    var strg = ViewToStringRenderer.RenderViewToString(this.ControllerContext, "~/Views/Users/EmailConfirmation.cshtml", User);

                    mailBody.AppendFormat(strg);
                    

                    Email.BodyEmail = mailBody.ToString();

                    MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                    mm.Subject = Email.SubjectEmail;
                    mm.Body = Email.BodyEmail;
                    mm.IsBodyHtml = true;
                    mm.BodyEncoding = Encoding.GetEncoding("utf-8");

                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mm);

                    UBL.InsertActivity(User.UserName, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(),DateTime.Now.AddHours(-6));

                    return this.RedirectToAction("RegisterConfirmation", "Account", new { FullName = User.FullName });
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

            return View(User);
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
        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string FullName)
        {
            ViewBag.FullName = FullName;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            if ((Request.IsAuthenticated))
            {
                return this.Redirect(ReturnUrl);
            }
            else
            {
                return this.View();
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string ReturnUrl, Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            Users LoginUser = UBL.Login(model);

            if (LoginUser.UserID >= 1)
            {
                UBL.InsertActivity(LoginUser.UserName, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));

                FormsAuthentication.SetAuthCookie(LoginUser.UserName, model.RememberMe);
                if (this.Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                    && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(ReturnUrl);
                }

                Geolocation location = GetGeolocation(model.IP);
                LoginRecord login = new LoginRecord()
                {
                    UserID = LoginUser.UserID,
                    IP = location.Ip,
                    Country = location.Location.Country,
                    Region = location.Location.Region,
                    City = location.Location.City
                };

                UBL.AddLogin(login);

                ViewBag.UserName = LoginUser.UserName;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (LoginUser.UserID == -1)
                {
                    this.ModelState.AddModelError(String.Empty, "El nombre de usuario o Contraseña es incorrecto.");
                }
                else
                {
                    this.ModelState.AddModelError(String.Empty, "El usuario aun no esta registrado.");
                }
            }
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult ForgotPasswordEmail(AuthorizationCode model)
        {
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            AuthorizationCode Code = UBL.AuthCode(model.Email);

            if (Code.UserID >= 1)
            {
                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = model.Email,
                    SubjectEmail = "Oasis Alajuela - Restablecer contraseña"
                };

                StringBuilder mailBody = new StringBuilder();
                var strg = ViewToStringRenderer.RenderViewToString(this.ControllerContext, "~/Views/Account/ForgotPasswordEmail.cshtml", Code);

                mailBody.AppendFormat(strg);

                Email.BodyEmail = mailBody.ToString();

                MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                mm.Subject = Email.SubjectEmail;
                mm.Body = Email.BodyEmail;
                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mm);

                ViewBag.GUID = Code.GUID;
                return this.RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            else
            {
                this.ModelState.AddModelError(String.Empty, "Este correo no esta registrado.");
            }

            return this.View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            int validation = UBL.ValidateGUID(id);

            if (validation == 0 || id == null)
            {
                ViewBag.Mensaje = "Este codigo de autorización es invalido o ya fue utilizado y esta obsoleto.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                ResetPasswordModel model = new ResetPasswordModel
                {
                    GUID = id
                };

                return View(model);
            }
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var r = UBL.ResetPassword(model);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return this.RedirectToAction("ResetPasswordConfirmation", "Account");
            }

        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        static Geolocation GetGeolocation(string IP)
        {

            string url = API_URL + $"apiKey={API_KEY}&ipAddress={IP}";
            string resultData = string.Empty;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resultData = reader.ReadToEnd();
            }

            Geolocation location = JsonConvert.DeserializeObject<Geolocation>(resultData);

            return location;
        }
    }
}