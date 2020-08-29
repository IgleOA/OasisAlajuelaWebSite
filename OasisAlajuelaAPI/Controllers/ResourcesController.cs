using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;
using System.Reflection;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;

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
        
    }
}