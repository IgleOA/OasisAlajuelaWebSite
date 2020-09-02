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

namespace OasisAlajuelaAPI.Controllers
{
    public class ResourcesController : ApiController
    {
        private ResourcesBL RBL = new ResourcesBL();

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