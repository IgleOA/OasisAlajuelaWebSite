using BL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class YouTubeController : ApiController
    {
        private YouTubeBL YBL = new YouTubeBL();

        [HttpGet]
        [ResponseType(typeof(List<YouTubeVideo>))]
        public HttpResponseMessage List()
        {
            var r = YBL.Youtubelist();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }
    }
}