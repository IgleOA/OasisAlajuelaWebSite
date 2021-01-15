using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using OasisAlajuelaAPI.Filters;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MainController : ApiController
    {
        private WebDirectoryBL WBL = new WebDirectoryBL();
        private UsersBL UBL = new UsersBL();
        private HomePageBL HBL = new HomePageBL();
        private AboutPageBL ABL = new AboutPageBL();

        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(List<WebDirectory>))]
        public HttpResponseMessage List([FromBody] WebDirectoryRequest Model)
        {
            List<WebDirectory> r = new List<WebDirectory>();

            r = WBL.WDByUser(Model);
            
            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Main/HomePage")]
        [ResponseType(typeof(List<HomePage>))]
        public HttpResponseMessage HomePage()
        {
            var HP = HBL.HomePage(true);

            return this.Request.CreateResponse(HttpStatusCode.OK, HP);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Main/HomePageDetail")]
        [ResponseType(typeof(List<HomePage>))]
        public HttpResponseMessage HomePageDetail()
        {
            var HP = HBL.HomePage(false);

            return this.Request.CreateResponse(HttpStatusCode.OK, HP);
        }

        //[HttpPost]
        //[ApiKeyAuthentication]
        //[Route("api/Main/UpdateHomePage")]
        //[ResponseType(typeof(bool))]
        //public HttpResponseMessage UpdateHomePage([FromBody] HomePage model)
        //{
        //    var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    var token = authHeader.Substring("Bearer ".Length);
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token);
        //    var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        //    var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

        //    var r = HBL.AddHomePage(model, UserName);
        //    if (!r)
        //    {
        //        return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
        //    }
        //    else
        //    {
        //        return this.Request.CreateResponse(HttpStatusCode.OK, r);
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("api/Main/AboutPage")]
        //[ResponseType(typeof(AboutPage))]
        //public HttpResponseMessage AboutPage()
        //{
        //    var HP = ABL.About();

        //    return this.Request.CreateResponse(HttpStatusCode.OK, HP);
        //}

        //[HttpPost]
        //[ApiKeyAuthentication]
        //[Route("api/Main/UpdateAboutPage")]
        //[ResponseType(typeof(bool))]
        //public HttpResponseMessage UpdateAboutPage([FromBody] AboutPage model)
        //{
        //    var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    var token = authHeader.Substring("Bearer ".Length);
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token);
        //    var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        //    var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

        //    var r = ABL.UpdateAboutPage(model, UserName);
        //    if (!r)
        //    {
        //        return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
        //    }
        //    else
        //    {
        //        return this.Request.CreateResponse(HttpStatusCode.OK, r);
        //    }
        //}

    }
}