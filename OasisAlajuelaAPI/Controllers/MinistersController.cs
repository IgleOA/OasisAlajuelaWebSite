using BL;
using ET;
using OasisAlajuelaAPI.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "https://oasisangular.azurewebsites.net", headers: "*", methods: "*")]
    public class MinistersController : ApiController
    {
        private MinistersBL MBL = new MinistersBL();

        [HttpPost]
        [ResponseType(typeof(List<Ministers>))]
        public HttpResponseMessage List()
        {
            var r = MBL.List(false);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [ResponseType(typeof(Ministers))]
        public HttpResponseMessage Details(int id)
        {
            var r = MBL.Details(id);

            if (r.MinisterID > 0)
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
        [Route("api/Ministers/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Ministers model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = MBL.Update(model, UserName);

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
        [Route("api/Ministers/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Ministers model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = MBL.AddNew(model, UserName);

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