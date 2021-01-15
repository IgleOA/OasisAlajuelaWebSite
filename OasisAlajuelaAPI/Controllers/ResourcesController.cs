using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;
using System.Reflection;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using OasisAlajuelaAPI.Filters;
using System.IO;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResourcesController : ApiController
    {
        private ResourcesBL RBL = new ResourcesBL();
        private UsersBL USBL = new UsersBL();

        [HttpPost]
        //[Route("api/Groups/ByUser")]
        [ResponseType(typeof(List<ResourceTypes>))]
        public HttpResponseMessage List(string id)
        {
            var r = RBL.TypeList(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        //[Route("api/Groups/ByUser")]
        [ResponseType(typeof(List<ResourceTypes>))]
        public HttpResponseMessage FullList()
        {
            var r = RBL.TypeList(string.Empty);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [Route("api/Resources/ResourcesList")]
        [ResponseType(typeof(ResourceTypes))]
        public HttpResponseMessage ResourcesList([FromBody] ResourceRequest model)
        {
            ResourceTypes Type = RBL.ResourceTypeDetail(model.ResourceTypeID);

            Type.TopResources = RBL.ResourceList(model.ResourceTypeID, model.StartDate);

            return this.Request.CreateResponse(HttpStatusCode.OK, Type);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Resources/History")]
        [ResponseType(typeof(ResourceTypes))]
        public HttpResponseMessage History(int id)
        {
            ResourceTypes Type = RBL.ResourceTypeDetail(id);

            Type.TopResources = RBL.History(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, Type);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Resources/Details")]
        [ResponseType(typeof(Resources))]
        public HttpResponseMessage Details(int id)
        {
            var r = RBL.ResourceDetails(id);

            if (r.ResourceTypeID > 0)
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
        [Route("api/Resources/AddNewRT")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNewRT([FromBody] ResourceTypes model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = RBL.AddNewResourceType(model, UserName);

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
        [Route("api/Resources/AddNewResource")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNewResource([FromBody] Resources model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            if (model.AccessLimited == true)
            {
                model.EnableStart = model.ESDate.Add(model.ESTime);
                model.EnableEnd = model.EEDate.Add(model.EETime);
            }
            var r = RBL.AddNewResource(model, UserName);

            if (r > 0)
            {
                #region Email
                Resources Res = RBL.ResourceDetails(r);
                MailAddressCollection emailtoBCC = new MailAddressCollection();
                List<Users> Subscribers = USBL.Subscribers(r, false);

                if (Subscribers.Count() > 0)
                {
                    foreach (var item in Subscribers)
                    {
                        emailtoBCC.Add(item.Email);
                    }
                }
                string body = string.Empty;         
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/NewResource.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{ResourceTypeID}", Res.ResourceTypeID.ToString());
                body = body.Replace("{TypeName}", Res.TypeName);
                body = body.Replace("{FileName}", Res.FileName);
                body = body.Replace("{Description}", Res.Description);

                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = ConfigurationManager.AppSettings["Subscribers"].ToString(),
                    SubjectEmail = "Oasis Alajuela ha subido un Recurso",
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

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Resources/UpdateResource")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage UpdateResource([FromBody] Resources model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            if (model.AccessLimited == true)
            {
                model.EnableStart = model.ESDate.Add(model.ESTime);
                model.EnableEnd = model.EEDate.Add(model.EETime);
            }

            var r = RBL.Update(model, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }

    }
}