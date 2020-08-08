using ET;
using BL;
using System.Collections.Generic;
using System.Web.Http;

namespace OasisAlajuelaAPI.Controllers
{
    public class ServicesController : ApiController
    {
        private ServicesBL SBL = new ServicesBL();

        [HttpGet]
        public IEnumerable<Services> List()
        {
            return SBL.List(false);
        }

        [HttpGet]
        public Services Details(int id)
        {
            return SBL.Details(id);
        }
    }
}