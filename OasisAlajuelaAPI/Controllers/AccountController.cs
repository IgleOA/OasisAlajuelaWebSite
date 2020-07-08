using BL;
using ET;
using OasisAlajuelaAPI.Filters;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OasisAlajuelaAPI.Controllers
{
    
    public class AccountController : ApiController
    {
        private UsersBL UBL = new UsersBL();
        private RolesBL RBL = new RolesBL();
        private GroupsBL GBL = new GroupsBL();

        [BasicAuthentication]
        [System.Web.Http.Route("api/Account/Login")]
        public Users Get()
        {
            var authenticationToken = Request.Headers.Authorization.Parameter;
            var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
            var usernamePasswordArray = decodedAuthenticationToken.Split(':');
            var userName = usernamePasswordArray[0];
            var password = usernamePasswordArray[1];

            Login login = new Login()
            {
                UserName = userName,
                Password = password
            };

            Users LoginUser = UBL.Login(login);

            Users Details = UBL.Details(LoginUser.UserID);

            Details.RolesData = RBL.List().Where(x => x.RoleID == Details.RoleID).FirstOrDefault();
            Details.GroupList = GBL.ListbyUser(Details.UserID);

            return Details;
        }

        [ApiKeyAuthentication]
        [System.Web.Http.Route("api/Account/CheckAvailability")]
        public bool Get(string id)
        {
            return UBL.CheckAvailability(id);
        }

        [ApiKeyAuthentication]
        [System.Web.Http.Route("api/Account/ForgotPassword")]
        public string Post([FromBody] ForgotPasswordModel model)
        {
            Users User = UBL.List().Where(x => x.Email == model.Email).FirstOrDefault();

            if (User.UserID > 0)
            {
                AuthorizationCode Code = UBL.AuthCode(model.Email);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/ForgotPassword.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{FullName}", User.FullName);
                body = body.Replace("{GUID}", Code.GUID);

                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = model.Email,
                    SubjectEmail = "Oasis Alajuela - Restablecer Contraseña",
                    BodyEmail = body
                };

                MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail)
                {
                    Subject = Email.SubjectEmail,
                    Body = Email.BodyEmail,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.GetEncoding("utf-8")
                };

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mm);
                return Code.GUID;
            }
            else
            {
                return string.Empty;
            }
        }

        [ApiKeyAuthentication]
        [System.Web.Http.Route("api/Account/ResetPassword")]
        public IHttpActionResult Put([FromBody] ResetPasswordModel model)
        {
            int validation = UBL.ValidateGUID(model.GUID);
            if (validation == 0)
            {
                return StatusCode(System.Net.HttpStatusCode.NotFound);
            }
            else
            {
                var r = UBL.ResetPassword(model);
                return Ok(r);
            }
        }

        [ApiKeyAuthentication]
        [System.Web.Http.Route("api/Account/Register")]
        public IHttpActionResult Post([FromBody] Users model)
        {
            model.RoleID = 1; /*New User*/
            var r = UBL.AddUser(model, model.FullName);
            if (!r)
            {                
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                string body = string.Empty;
                using(StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/NewUser.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{FullName}", model.FullName);
                body = body.Replace("{UserName}", model.UserName);
                body = body.Replace("{Password}", model.Password);

                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = model.Email,
                    SubjectEmail = "Oasis Alajuela - Registro satisfactorio",
                    BodyEmail = body
                };

                MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail)
                {
                    Subject = Email.SubjectEmail,
                    Body = Email.BodyEmail,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.GetEncoding("utf-8")
                };

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mm);
                Users NewUser = UBL.List().Where(x => x.UserName == model.UserName).FirstOrDefault();
                return Ok(NewUser);
            }
        }

        

    }
}