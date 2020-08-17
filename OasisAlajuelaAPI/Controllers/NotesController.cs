using ET;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OasisAlajuelaAPI.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace OasisAlajuelaAPI.Controllers
{
    [ApiKeyAuthentication]
    public class NotesController : ApiController
    {
        private UserNotesBL UNBL = new UserNotesBL();

        [HttpPost]
        [Route("api/Notes/AddUserNote")]
        [ResponseType(typeof(Boolean))]
        public HttpResponseMessage AddUserNote([FromBody] UserNoteRequest model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            try
            {
                foreach (var item in model.UserID)
                {
                    UserNotes UG = new UserNotes
                    {
                        UserID = item,
                        RequestNote = model.RequestNote,
                        ResponseRequired = model.ResponseRequired
                    };
                    var r = UNBL.AddNote(UG, UserName);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}