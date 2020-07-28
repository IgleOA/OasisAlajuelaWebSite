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

        public IEnumerable<Leadership> Get()
        {
            return LBL.List();            
        }
        
        public Leadership Get(int id)
        {
            return LBL.Details(id);
        }
    }
}