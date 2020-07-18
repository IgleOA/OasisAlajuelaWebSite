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

        public IEnumerable<Sermons> Get()
        {
            return SBL.List(false);
        }

        public Sermons Get(int id)
        {
            return SBL.Details(id);
        }
    }
}