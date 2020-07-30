using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;
using System.Reflection;

namespace OasisAlajuelaAPI.Controllers
{
    public class ResourcesController : ApiController
    {
        private ResourcesBL RBL = new ResourcesBL();

        public IEnumerable<ResourceTypes> Get(string id)
        {
            return RBL.TypeList(id);
        }

        public ResourceTypes Post([FromBody] ResourceRequest model)
        {
            ResourceTypes Type = RBL.ResourceTypeDetail(model.ResourceTypeID);

            Type.TopResources = RBL.ResourceList(model.ResourceTypeID, model.StartDate);

            return Type;
        }
    }
}