using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ET;
using BL;

namespace OasisAlajuelaAPI.Controllers
{
    public class LeadershipController : ApiController
    {
        private LeadershipBL LBL = new LeadershipBL();

        [HttpGet]
        public IEnumerable<Leadership> List()
        {
            return LBL.List();            
        }
        
        [HttpGet]
        public Leadership Details(int id)
        {
            return LBL.Details(id);
        }
    }
}