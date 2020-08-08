using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;

namespace OasisAlajuelaAPI.Controllers
{
    public class BlogController : ApiController
    {
        private BlogsBL BBL = new BlogsBL();
                
        [HttpGet]
        public IEnumerable<Blogs> List()
        {
            return BBL.List();
        }

        [HttpGet]
        public Blogs Details(int id)
        {
            return BBL.Details(id);
        }
    }
}