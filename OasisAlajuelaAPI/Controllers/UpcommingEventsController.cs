using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using System.Configuration;

namespace OasisAlajuelaAPI.Controllers
{
    public class UpcommingEventsController : ApiController
    {
        private UpcommingEventsBL UBL = new UpcommingEventsBL();

        [Route("api/UpcommingEvents/Next")]
        public UpcommingEvents Get(DateTime id)
        {
            return UBL.List(id, false, true).Take(1).FirstOrDefault();
        }

        
    }
}