using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using OasisAlajuelaAPI.Filters;
using System.Web.Http.Description;
using System.Web;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BannersController : ApiController
    {
        private BannersBL BBL = new BannersBL();
        private BannersLocationBL BLBL = new BannersLocationBL();
        private TokensBL TBL = new TokensBL();
        private UsersBL UBL = new UsersBL();

        [HttpGet]
        public IEnumerable<Banner> List(string id)
        {
            return BBL.Banners(id, false);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Banners/ChangeStatus")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage ChangeStatus([FromBody] Banner model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = BBL.Update(model.BannerID, UserName);

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
        [Route("api/Banners/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Banner model)
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

        [HttpGet]
        [Route("api/Banners/Locations")]
        [ResponseType(typeof(List<BannersLocation>))]
        public HttpResponseMessage BannersLocationList()
        {
            var list = BLBL.List();

            return this.Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}