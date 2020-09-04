using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;
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
    public class BlogController : ApiController
    {
        private BlogsBL BBL = new BlogsBL();

        [HttpPost]
        [ResponseType(typeof(List<Blogs>))]
        public HttpResponseMessage List()
        {
            var r = BBL.List();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Blog/History")]
        [ResponseType(typeof(List<Blogs>))]
        public HttpResponseMessage History()
        {
            var r = BBL.History();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [ResponseType(typeof(Blogs))]
        public HttpResponseMessage Details(int id)
        {
            var r = BBL.Details(id);

            if (r.BlogID > 0)
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
        [Route("api/Blog/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Blogs model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = BBL.Update(model, UserName);

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
        [Route("api/Blog/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Blogs model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = BBL.AddNew(model, UserName);

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