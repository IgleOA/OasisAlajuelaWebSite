using System;
using System.Web.Mvc;
using System.Net.Mail;
using ET;
using BL;
using System.Web.Security;
using OasisAlajuelaWebSite.Models;
using System.Text;

namespace OasisAlajuelaWebSite.Controllers
{ 
    [AllowSameSite]
    [Authorize]
    public class AccountController : Controller
    {
        private UsersBL UBL = new UsersBL();

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
                        FromEmail = "johmstone@gmail.com",
                        ToEmail = User.Email,
                        SubjectEmail = "Oasis Alajuela - Registro satisfactorio",
                        BodyEmail = "Gracias " + User.FullName + " por registrarse y por tener el sentir de hacerte parte de esta familia. Dios trae cosas grandes para esta casa y ahora seras parte de ellas. Bendiciones..."
                    };

                    StringBuilder mailBody = new StringBuilder();

                    mailBody.AppendFormat("<h1>Oasis Alajuela</h1>");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Gracias {0} por registrarse y por tener el sentir de hacerte parte de esta familia. Dios trae cosas grandes para esta casa y ahora seras parte de ellas.</p>", User.FullName);
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Desde ya puedes ver el contenido completo de nuestro website http://igleoa.azurewebsites.net/ </p>");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<h3>Bendiciones....</h3>");

                    Email.BodyEmail = mailBody.ToString();

                    MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                    mm.Subject = Email.SubjectEmail;
                    mm.Body = Email.BodyEmail;
                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mm);

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
                FormsAuthentication.SetAuthCookie(LoginUser.UserName, model.RememberMe);
                if (this.Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                    && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(ReturnUrl);
                }

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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            AuthorizationCode Code = UBL.AuthCode(model.Email);

            if (Code.UserID >= 1)
            {
                Emails Email = new Emails();

                Email.FromEmail = "johmstone@gmail.com";
                Email.ToEmail = model.Email;
                Email.SubjectEmail = Code.FullName + " - Restablecer contraseña";
                Email.BodyEmail = "Para restablecer su contraseña, utilice el siguiente link http://igleoa.azurewebsites.net/Account/ResetPassword?GUID=" + Code.GUID;
                //Email.BodyEmail = "Para restablecer su contraseña, utilice el siguiente link http://localhost:61214/Account/ResetPassword?GUID=" + Code.GUID;

                MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail);
                mm.Subject = Email.SubjectEmail;
                mm.Body = Email.BodyEmail;
                mm.IsBodyHtml = false;

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
        public ActionResult ResetPassword(string GUID)
        {
            int validation = UBL.ValidateGUID(GUID);

            if (validation == 0 || GUID == null)
            {
                return View("Este codigo de autorización es invalido o ya fue utilizado y esta obsoleto.");
            }

            return View();

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
    }
}