using ET;
using BL;
using OasisAlajuelaAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;

namespace OasisAlajuelaAPI.Controllers
{
    public class UsersController : ApiController
    {
        private RightsBL RRBL = new RightsBL();
        private TokensBL TBL = new TokensBL();

        [ApiKeyAuthentication]
        [Route("api/Users/RightsValidation")]
        [ResponseType(typeof(AccessRights))]
        public HttpResponseMessage Post([FromBody] AccessRightsRequest model)
        {
            AccessRights Result = RRBL.ValidationRights(model.UserName, model.Controller, model.Action);
            
            return this.Request.CreateResponse(HttpStatusCode.OK, Result);
        }


        [Route("api/Users/TokenValidation")]
        [ResponseType(typeof(Token))]
        public HttpResponseMessage Post(string AccessType)
        {
            var authHeader = this.ActionContext.Request.Headers.GetValues(AccessType).FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);

            Token r = TBL.ValidateToken(token);

            return this.Request.CreateResponse(HttpStatusCode.OK,r);
        }
    }
}