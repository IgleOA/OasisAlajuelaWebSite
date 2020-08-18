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
        private UsersBL UBL = new UsersBL();
        private ResourcesBL RBL = new ResourcesBL();

        [HttpPost]
        //[Route("api/Groups/ByUser")]
        [ResponseType(typeof(List<Groups>))]
        public HttpResponseMessage FullList()
        {
            var r = GBL.FullList();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        //[Route("api/Groups/ByUser")]
        [ResponseType(typeof(Groups))]
        public HttpResponseMessage Details(int id)
        {
            var r = GBL.Details(id);

            r.UserList = GBL.UserList(id);
            r.RTypesList = GBL.RTList(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [Route("api/Groups/MissingUsersByGroup")]
        [ResponseType(typeof(List<Users>))]
        public HttpResponseMessage MissingUsersByGroup(int id)
        {
            var data = from r in UBL.List()
                       where !(from d in GBL.UserList(id)
                               select d.UserID).Contains(r.UserID)
                       select r;

            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [HttpPost]
        [Route("api/Groups/MissingRTByGroup")]
        [ResponseType(typeof(List<ResourceTypes>))]
        public HttpResponseMessage MissingRTByGroup(int id)
        {
            var data = from r in RBL.TypeList(string.Empty)
                       where !(from d in GBL.RTList(id)
                               select d.ResourceTypeID).Contains(r.ResourceTypeID)
                       select r;

            return this.Request.CreateResponse(HttpStatusCode.OK, data);
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
        [Route("api/Groups/GroupsbyUser")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage GroupsbyUser([FromBody] GroupsbyUserRequest model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            Boolean r = false;
            try
            {
                if (model.ActionType == "ADD")
                {
                    foreach (var item in model.GroupID)
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

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        [Route("api/Groups/UsersbyGroup")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage UsersbyGroup([FromBody] UsersbyGroupRequest model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            Boolean r = false;
            try
            {
                if (model.ActionType == "ADD")
                {
                    foreach (var item in model.UserID)
                    {
                        UsersGroups UG = new UsersGroups()
                        {
                            UserID = item,
                            ActionType = model.ActionType,
                            GroupID = model.GroupID
                        };
                        r = GBL.AddUserGroup(UG, UserName);
                    }

                }
                else
                {
                    UsersGroups UG = new UsersGroups()
                    {
                        UserGroupID = model.UserGroupID,
                        ActionType = model.ActionType
                    };
                    r = GBL.RemoveUG(UG, UserName);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        [Route("api/Groups/RTypesbyGroup")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage RTypesbyGroup([FromBody] RTypesbyGroupRequest model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            Boolean r = false;
            try
            {
                if (model.ActionType == "ADD")
                {
                    foreach (var item in model.ResourceTypeID)
                    {
                        ResourcesGroups UG = new ResourcesGroups
                        {
                            ResourceTypeID = item,
                            GroupID = model.GroupID
                        };
                        r = GBL.AddRTGroup(UG, UserName);
                    }
                }
                else
                {
                    ResourcesGroups UG = new ResourcesGroups()
                    {
                        ResourceGroupID = model.ResourceGroupID
                    };
                    r = GBL.RemoveRG(UG, UserName);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Groups/Edit")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Edit([FromBody] Groups model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = GBL.Update(model, UserName);

            if (r)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Groups/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Groups model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = GBL.AddNew(model, UserName);

            if (r)
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