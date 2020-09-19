using ET;
using BL;
using OasisAlajuelaAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [ApiKeyAuthentication]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WebDirectoryController : ApiController
    {
        private WebDirectoryBL WBL = new WebDirectoryBL();

        [HttpGet]
        [ApiKeyAuthentication]
        [ResponseType(typeof(List<WebDirectory>))]
        public HttpResponseMessage List(int id)
        {
            var r = WBL.List(id);
            if (r.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);                
            }
        }

        [HttpPost]
        [Route("api/WebDirectory/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] WebDirectory model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = WBL.AddNew(model,UserName);

            if (r)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}