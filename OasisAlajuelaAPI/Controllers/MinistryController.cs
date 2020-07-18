using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;
using System.Linq;

namespace OasisAlajuelaAPI.Controllers
{
    public class MinistryController : ApiController
    {
        private MinistriesBL MBL = new MinistriesBL();

        [AllowAnonymous]
        public IEnumerable<Ministries> Get(int id)
        {
            if (id == 0)
            {
                return MBL.List();
            }
            else
            {
                return MBL.List().Where(x => x.MinistryID == id);
            }
        }        
    }
}