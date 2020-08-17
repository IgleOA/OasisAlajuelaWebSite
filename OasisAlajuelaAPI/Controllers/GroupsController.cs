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
        //[Route("api/Groups/ByUser")]
        [ResponseType(typeof(List<Groups>))]
        public HttpResponseMessage FullList()
        {
            var r = GBL.FullList();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [Route("api/Groups/MissingListByUser")]
        [ResponseType(typeof(List<Groups>))]
        public HttpResponseMessage MissingListByUser(int id)
        {
            var data = from r in GBL.List()
                       where !(from d in GBL.ListbyUser(id)
                               select d.GroupID).Contains(r.GroupID)
                       select r;

            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

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
        public HttpResponseMessage UserGroup([FromBody] UsersGroupsRequest model)
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
                foreach(var item in model.GroupID)
                {
                    UsersGroups UG = new UsersGroups()
                    {
                        UserID = model.UserID,
                        ActionType = model.ActionType,
                        GroupID = item
                    };
                    r = GBL.AddUserGroup(UG, UserName);
                }
                
            }
            else
            {
                foreach (var item in model.GroupID)
                {
                    UsersGroups UG = new UsersGroups()
                    {
                        UserID = model.UserID,
                        ActionType = model.ActionType,
                        GroupID = item
                    };
                    r = GBL.RemoveUG(UG, UserName);
                }
                
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