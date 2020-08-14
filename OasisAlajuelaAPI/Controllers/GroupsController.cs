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

namespace OasisAlajuelaAPI.Controllers
{
    [ApiKeyAuthentication]
    public class GroupsController : ApiController
    {
        private GroupsBL GBL = new GroupsBL();

        [HttpPost]
        [Route("api/Groups/ByUser")]
        [ResponseType(typeof(List<Groups>))]
        public HttpResponseMessage ByUser(int id)
        {
            var r = GBL.ListbyUser(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [Route("api/Groups/UserGroup")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage UserGroup([FromBody] UsersGroups model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            Boolean r = false;

            if (model.ActionType == "ADD")
            {
                r = GBL.AddUserGroup(model, UserName);
            }
            else
            {
                r = GBL.RemoveUG(model, UserName);
            }

            if(r)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            
        }       

    }
}