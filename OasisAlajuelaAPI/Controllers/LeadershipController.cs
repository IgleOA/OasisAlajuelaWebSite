using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ET;
using BL;
using OasisAlajuelaAPI.Filters;
using System.Web.Http.Description;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LeadershipController : ApiController
    {
        private LeadershipBL LBL = new LeadershipBL();

        [HttpGet]
        public IEnumerable<Leadership> List()
        {
            return LBL.List();            
        }
        
        [HttpGet]
        public Leadership Details(int id)
        {
            return LBL.Details(id);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Leadership/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Leadership model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = LBL.Update(model, UserName);

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
        [Route("api/Leadership/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Leadership model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = LBL.AddNew(model, UserName);

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