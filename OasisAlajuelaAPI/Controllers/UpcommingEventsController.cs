using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using System.Configuration;
using System.Web.Http.Description;
using OasisAlajuelaAPI.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "https://oasisangular.azurewebsites.net", headers: "*", methods: "*")]
    public class UpcommingEventsController : ApiController
    {
        private UpcommingEventsBL UBL = new UpcommingEventsBL();

        [HttpPost]
        [ResponseType(typeof(List<UpcommingEvents>))]
        public HttpResponseMessage List([FromBody] UpcommingEventsRequest Request)
        {
            var r =  UBL.List(Request.Startdate, Request.UpCommingFlag, Request.ActiveFlag);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpGet]
        [Route("api/UpcommingEvents/Next")]
        [ResponseType(typeof(UpcommingEvents))]
        public HttpResponseMessage NextEvent(DateTime id)
        {
            var r = UBL.List(id, false, true).Take(1).FirstOrDefault();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);

        }        
        
        [HttpPost]
        [ApiKeyAuthentication]
        [ResponseType(typeof(Services))]
        public HttpResponseMessage Details(int id)
        {
            var r = UBL.Details(id);

            if (r.EventID > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/UpcommingEvents/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] UpcommingEvents model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            model.ScheduledDate = model.ScheduledDate.Add(model.ScheduledTime);

            var r = UBL.Update(model, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/UpcommingEvents/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] UpcommingEvents model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            model.ScheduledDate = model.ScheduledDate.Add(model.ScheduledTime);

            var r = UBL.AddNew(model, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }
    }
}