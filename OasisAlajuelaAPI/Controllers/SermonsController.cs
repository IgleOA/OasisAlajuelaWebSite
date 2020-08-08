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
    public class SermonsController : ApiController
    {
        private SermonsBL SBL = new SermonsBL();

        [HttpGet]
        public IEnumerable<Sermons> List()
        {
            return SBL.List(false);
        }

        [HttpGet]
        public Sermons Details(int id)
        {
            return SBL.Details(id);
        }
    }
}