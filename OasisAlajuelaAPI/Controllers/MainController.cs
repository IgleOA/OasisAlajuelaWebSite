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
    public class MainController : ApiController
    {
        private WebDirectoryBL WBL = new WebDirectoryBL();
        private UsersBL UBL = new UsersBL();
        private HomeBL HBL = new HomeBL();

        // GET api/<controller>
        [AllowAnonymous]
        public IEnumerable<WebDirectory> Get(int id)
        {
            string username = "";

            if(id > 0)
            {
                username = UBL.Details(id).UserName;
            }

            return WBL.WDByUser(username);
        }

        [Route("api/Main/Home")]
        public HomePage Get()
        {
            return HBL.Home();
        }
        
    }
}