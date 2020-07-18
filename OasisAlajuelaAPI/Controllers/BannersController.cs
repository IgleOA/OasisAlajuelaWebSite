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
    public class BannersController : ApiController
    {
        private BannersBL BBL = new BannersBL();
        
        public IEnumerable<Banner> Get(string id)
        {
            return BBL.Banners(id, false);
        }
    }
}