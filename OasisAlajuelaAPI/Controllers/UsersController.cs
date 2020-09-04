using ET;
using BL;
using OasisAlajuelaAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "https://oasisangular.azurewebsites.net", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private RightsBL RRBL = new RightsBL();
        private TokensBL TBL = new TokensBL();
        private UserProfileBL UPBL = new UserProfileBL();
        private GroupsBL GBL = new GroupsBL();
        private UsersBL UBL = new UsersBL();

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/RightsValidation")]
        [ResponseType(typeof(AccessRights))]
        public HttpResponseMessage RightsValidation([FromBody] AccessRightsRequest model)
        {
            AccessRights Result = RRBL.ValidationRights(model.UserName, model.Controller, model.Action);
            
            return this.Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpPost]
        [Route("api/Users/TokenValidation")]
        [ResponseType(typeof(Token))]
        public HttpResponseMessage TokenValidation(string AccessType)
        {
            var authHeader = this.ActionContext.Request.Headers.GetValues(AccessType).FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);

            Token r = TBL.ValidateToken(token);

            return this.Request.CreateResponse(HttpStatusCode.OK,r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/Profile/")]
        [ResponseType(typeof(UserProfile))]
        public HttpResponseMessage Profile([FromBody] int id)
        {
            UserProfile r = UPBL.Detail(id);

            if (r.UserID >= 1)
            {
                r.GroupList = GBL.ListbyUser(id);

                if (r.LastActivityDate.ToString().Length == 0)
                {
                    r.LastActivityDate = r.CreationDate;
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/Profile/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage UpdateProfile([FromBody] UserProfile model)
        {
            var r = UPBL.Update(model, model.UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        //[Route("api/Users")]
        [ResponseType(typeof(List<Users>))]
        public HttpResponseMessage List()
        {
            var r = UBL.List();

            if (r.Count > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);                
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        //[Route("api/Users")]
        [ResponseType(typeof(Users))]
        public HttpResponseMessage Details(int id)
        {
            var r = UBL.Details(id);

            if (r.UserID > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Users model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = UBL.Update(model, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/AdminResetPassword")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AdminResetPassword([FromBody] int id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = UBL.AdminResetPassword(id, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                var model = UBL.Details(id);

                #region Email
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/AdminResetPassword.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{FullName}", model.FullName);
                body = body.Replace("{UserName}", model.UserName);
                body = body.Replace("{Password}", "!234s6789");

                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = model.Email,
                    SubjectEmail = "Su contraseña de OasisAlajuela.com ha sido restablecida",
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
                #endregion

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/AddUser/")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddUser([FromBody] Users model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            model.Password = "Wxyz1234";

            var r = UBL.AddUser(model, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/NewUser.html")))
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

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            
        }
    }
}