using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using System.Web.Http.Description;
using OasisAlajuelaAPI.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "https://oasisangular.azurewebsites.net", headers: "*", methods: "*")]
    public class SermonsController : ApiController
    {
        private SermonsBL SBL = new SermonsBL();
        private UsersBL USBL = new UsersBL();

        [HttpPost]
        [ResponseType(typeof(List<Sermons>))]
        public HttpResponseMessage List()
        {
            var r = SBL.List(true);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Sermons/History")]
        [ResponseType(typeof(List<Sermons>))]
        public HttpResponseMessage History()
        {
            var r = SBL.List(false);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [ResponseType(typeof(Sermons))]
        public HttpResponseMessage Details(int id)
        {
            var r = SBL.Details(id);

            if (r.SermonID > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Sermons/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Sermons model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = SBL.Update(model, UserName);

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
        [Route("api/Sermons/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Sermons model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = SBL.AddNew(model, UserName);

            if (r > 0)
            {
                #region Email
                SermonEmail Res = SBL.DetailsForEmail(r);
                MailAddressCollection emailtoBCC = new MailAddressCollection();
                List<Users> Subscribers = USBL.Subscribers(0, true);

                if (Subscribers.Count() > 0)
                {
                    foreach (var item in Subscribers)
                    {
                        emailtoBCC.Add(item.Email);
                    }
                }
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/NewSermons.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{Title}", Res.Title);
                body = body.Replace("{MinisterName}", Res.MinisterName);
                body = body.Replace("{Description}", Res.Description);

                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = ConfigurationManager.AppSettings["Subscribers"].ToString(),
                    SubjectEmail = "Oasis Alajuela ha subido una Prédica",
                    BodyEmail = body
                };

                MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail)
                {
                    Subject = Email.SubjectEmail,
                    Body = Email.BodyEmail,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.GetEncoding("utf-8")
                };

                if (Subscribers.Count() > 0)
                {
                    mm.Bcc.Add(emailtoBCC.ToString());
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mm);
                #endregion
                return this.Request.CreateResponse(HttpStatusCode.OK, true);                
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}