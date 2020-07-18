using ET;
using BL;
using System.Collections.Generic;
using System.Web.Http;

namespace OasisAlajuelaAPI.Controllers
{
    public class ServicesController : ApiController
    {
        private ServicesBL SBL = new ServicesBL();

        public IEnumerable<Services> Get()
        {
            return SBL.List(false);
        }

        public Services Get(int id)
        {
            return SBL.Details(id);
        }
    }
}