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
using System.Web.Http.Cors;

namespace OasisAlajuelaAPI.Controllers
{
    [ApiKeyAuthentication]
    [EnableCors(origins: "https://oasisangular.azurewebsites.net", headers: "*", methods: "*")]
    public class NotesController : ApiController
    {
        private UserNotesBL UNBL = new UserNotesBL();

        [HttpPost]
        //[Route("api/Notes/AddUserNote")]
        [ResponseType(typeof(List<UserNotes>))]
        public HttpResponseMessage List(bool id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            try
            {
                var r = UNBL.List(UserName, id);

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        

        [HttpPost]
        [Route("api/Notes/AddUserNote")]
        [ResponseType(typeof(bool))]
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

        [HttpPost]
        [Route("api/Notes/ReadNote")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage ReadNote(int id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            try
            {
                ResponseUserNote Note = new ResponseUserNote()
                {
                    NoteID = id,
                    ResponseNote = "READ MESSAGE",
                    ActionType = "READ"
                };

                var r = UNBL.UpdateNote(Note, UserName);

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Notes/ResponseNote")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage ResponseNote([FromBody] ResponseUserNote model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            try
            {
                model.ActionType = "UPDATE";

                var r = UNBL.UpdateNote(model, UserName);

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}