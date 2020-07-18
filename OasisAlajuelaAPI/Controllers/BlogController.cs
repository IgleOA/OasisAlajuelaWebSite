using System.Collections.Generic;
using System.Web.Http;
using ET;
using BL;

namespace OasisAlajuelaAPI.Controllers
{
    public class BlogController : ApiController
    {
        private BlogsBL BBL = new BlogsBL();
                
        public IEnumerable<Blogs> Get()
        {
            return BBL.List();
        }

        public Blogs Get(int id)
        {
            return BBL.Details(id);
        }
    }
}