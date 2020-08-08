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

        [HttpGet]
        public News Details(int id)
        {
            return NBL.Details(id);
            
        }

        [HttpGet]
        [Route("api/New/List")]
        public IEnumerable<News> List(bool id)
        {
            return NBL.List(id);            
        }


    }
}