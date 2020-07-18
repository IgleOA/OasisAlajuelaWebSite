using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;

namespace OasisAlajuelaAPI.Controllers
{
    public class NewController : ApiController
    {
        private NewsBL NBL = new NewsBL();

        public IEnumerable<News> Get(int id)
        {
            if (id == 0)
            {
                return NBL.List(false);
            }
            else
            {
                return NBL.List(false).Where(x => x.NewID == id);
            }
        }

        
    }
}