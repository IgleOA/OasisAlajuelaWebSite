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

        [AllowAnonymous]
        public IEnumerable<Leadership> Get(int id)
        {
            if (id == 0)
            {
                return LBL.List().Where(x => x.ActionLink.Length > 0);
            } 
            else 
            { 
                return LBL.List();
            }
        }        
    }
}