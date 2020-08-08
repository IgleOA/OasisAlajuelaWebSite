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

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<WebDirectory> List(int id)
        {
            string username = "";

            if(id > 0)
            {
                username = UBL.Details(id).UserName;
            }

            return WBL.WDByUser(username);
        }

        [HttpGet]
        [Route("api/Main/Home")]
        public HomePage HomePage()
        {
            return HBL.Home();
        }
        
    }
}