using BL;
using ET;
using OasisAlajuelaAPI.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiKeyAuthentication]
    public class EnrollmentsController : ApiController
    {
        private EnrollmentsBL EBL = new EnrollmentsBL();

        [HttpPost]
        [ApiKeyAuthentication]
        [ResponseType(typeof(List<Enrollments>))]
        public HttpResponseMessage List()
        {
            var r = EBL.List(true);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Enrollments/Active")]
        [ResponseType(typeof(List<Enrollments>))]
        public HttpResponseMessage ActiveEnrollments()
        {
            var r = EBL.List(false);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [Route("api/Enrollments/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Enrollments model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = EBL.Add(model, UserName);

            if (r)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

    }
}