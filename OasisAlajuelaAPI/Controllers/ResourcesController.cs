using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;

namespace OasisAlajuelaAPI.Controllers
{
    public class ResourcesController : ApiController
    {
        private ResourcesBL RBL = new ResourcesBL();

        public IEnumerable<ResourceTypes> Get(string id)
        {
            return RBL.TypeList(id);
        }
        
    }
}