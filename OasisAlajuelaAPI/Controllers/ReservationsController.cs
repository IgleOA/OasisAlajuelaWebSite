using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using System.Web.Http.Description;
using OasisAlajuelaAPI.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Cors;
using RotativaHQ.MVC5;

namespace OasisAlajuelaAPI.Controllers
{
    [ApiKeyAuthentication]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReservationsController : ApiController
    {
        private ReservationsBL RBL = new ReservationsBL();
        private UpcommingEventsBL UPL = new UpcommingEventsBL();

        [HttpPost]
        [Route("api/Reservations/AddNew")]
        [ResponseType(typeof(List<ReservationResult>))]
        public HttpResponseMessage AddNew([FromBody] ReservationRequest model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = RBL.AddNew(model, UserName);

            if (r.Count > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);                
            }
        }

        [HttpPost]
        [Route("api/Reservations")]
        [ResponseType(typeof(List<Reservations>))]
        public HttpResponseMessage List([FromBody] ReservationListRequest model)
        {
            var r = RBL.List(model);

            if (r.Count > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        //[HttpPost]
        //[Route("api/ReservationsPDF")]
        //public HttpResponseMessage ReservationsPDF(int id)
        //{
        //    ReservationListRequest ListRequest = new ReservationListRequest()
        //    {
        //        EventID = id,
        //        GUID = null
        //    };

        //    List<Reservations> ReservationList = new List<Reservations>();

        //    var EventDetails = UPL.Details(id);

        //    ReservationList = RBL.List(ListRequest);

        //    string filename = "Reservaciones_" + EventDetails.Title;

        //    return new ViewAsPdf("~/Home/ReservationsPrintVersion", ReservationList) { FileName = filename };
            
        //}
    }
}