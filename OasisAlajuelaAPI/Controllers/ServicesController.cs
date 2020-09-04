using ET;
using BL;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;
using OasisAlajuelaAPI.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "https://oasisangular.azurewebsites.net", headers: "*", methods: "*")]
    public class ServicesController : ApiController
    {
        private ServicesBL SBL = new ServicesBL();

        [HttpPost]
        [ResponseType(typeof(List<Services>))]
        public HttpResponseMessage List()
        {
            var r = SBL.List(false);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [ResponseType(typeof(Services))]
        public HttpResponseMessage Details(int id)
        {
            var r = SBL.Details(id);

            if (r.ServiceID > 0)
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
        [Route("api/Services/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Services model)
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
        [Route("api/Services/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Services model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = SBL.AddNew(model, UserName);

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