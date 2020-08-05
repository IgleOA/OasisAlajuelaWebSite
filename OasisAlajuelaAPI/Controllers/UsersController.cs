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
        private UserProfileBL UPBL = new UserProfileBL();
        private GroupsBL GBL = new GroupsBL();

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

        [ApiKeyAuthentication]
        [Route("api/Users/Profile/")]
        [ResponseType(typeof(UserProfile))]
        public HttpResponseMessage Post([FromBody] int id)
        {
            UserProfile r = UPBL.Detail(id);

            if (r.UserID >= 1)
            {
                r.GroupList = GBL.ListbyUser(id);

                if (r.LastActivityDate.ToString().Length == 0)
                {
                    r.LastActivityDate = r.CreationDate;
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [ApiKeyAuthentication]
        [Route("api/Users/Profile/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Post([FromBody] UserProfile model)
        {
            var r = UPBL.Update(model, model.UserName);

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