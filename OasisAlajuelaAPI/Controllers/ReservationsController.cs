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
using shortid;
using shortid.Configuration;
using System.Net.Mail;
using System.IO;
using System.Web;
using System.Configuration;
using System.Text;

namespace OasisAlajuelaAPI.Controllers
{
    [ApiKeyAuthentication]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReservationsController : ApiController
    {
        private AuditoriumLayoutBL ABL = new AuditoriumLayoutBL();
        private ReservationEventDetailBL WBL = new ReservationEventDetailBL();
        private UsersBL USBL = new UsersBL();
        private ReservationsBL RBL = new ReservationsBL();
        //private RightsBL RRBL = new RightsBL();
        //private UserProfileBL UPBL = new UserProfileBL();

        [HttpPost]
        [Route("api/Reservations/EventDetails")]
        [ResponseType(typeof(ReservationEventDetail))]
        public HttpResponseMessage EventDetails(int id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;

            ReservationEventDetail Model = WBL.Details(id, Convert.ToInt32(UserID));

            Model.Layout = ABL.Layout(id);
            
            if (Model.EventID > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, Model);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("api/Reservations/PreviousReserves")]
        [ResponseType(typeof(List<ReservationLevel1>))]
        public HttpResponseMessage PreviousReserves(int id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;

            var r = RBL.ReservationsMainInfo(id, Convert.ToInt32(UserID));

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
            
        }

        [HttpPost]
        [Route("api/Reservations/CheckOut")]
        [ResponseType(typeof(Reservations))]
        public HttpResponseMessage CheckOut([FromBody] ReservationEventDetail Model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;
            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var Options = new GenerationOptions
            {
                UseNumbers = true,
                UseSpecialCharacters = false
            };

            Reservations Reservation = new Reservations()
            {
                GUID = ShortId.Generate(Options),
                EventID = Model.EventID,
                BookedBy = Convert.ToInt32(UserID),
                BookedFor = (string.IsNullOrEmpty(Model.ReservedFor)) ? UserName : Model.ReservedFor,
                SeatsReserved = Model.SeatsReserved
            };

            Reservation.Details = RBL.AddReservation(Reservation, UserName);

            if (Reservation.Details.Count() >0)
            {
                #region Email
                MailAddressCollection emailtoBCC = new MailAddressCollection();
                List<Users> Subscribers = USBL.Subscribers(0, true);

                if (Subscribers.Count() > 0)
                {
                    foreach (var item in Subscribers)
                    {
                        emailtoBCC.Add(item.Email);
                    }
                }
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/ConfirmReservation.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{GUID}", Reservation.GUID);
                body = body.Replace("{BookedByName}", UserName);
                body = body.Replace("{BookedFor}", Reservation.BookedFor);

                var seats = string.Empty;
                foreach(var item in Reservation.Details)
                {
                    var code = "<tr><td class=\"p-0 text-center\" align=\"center\">" + item.SeatID
                                + "</td><td class=\"p-0 text-center\" align=\"center\">" + item.SeatID.Substring(0, 2)
                                + "</td><td class=\"p-0 text-center\" align=\"center\">" + item.SeatID.Substring(2, 1)
                                + "</td>";
                    var code2 = "<td class=\"p-0 text-center\" align=\"center\">" + item.SeatID.Substring(4, 1) + "</td>";
                    if (item.SeatID.Length >= 6)
                    {
                        code2 = "<td class=\"p-0 text-center\" align=\"center\">" + item.SeatID.Substring(4, 2) + "</td>";
                    }

                    code = code + code2 + "</tr>";
                    seats += code;
                }

                body = body.Replace("{Seats}", seats);


                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = ConfigurationManager.AppSettings["Subscribers"].ToString(),
                    SubjectEmail = "Oasis Alajuela - Confirmación de Reserva",
                    BodyEmail = body
                };

                MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail)
                {
                    Subject = Email.SubjectEmail,
                    Body = Email.BodyEmail,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.GetEncoding("utf-8")
                };

                if (Subscribers.Count() > 0)
                {
                    mm.Bcc.Add(emailtoBCC.ToString());
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mm);
                #endregion

                return this.Request.CreateResponse(HttpStatusCode.OK, Reservation);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        [HttpPost]
        [Route("api/Reservations/ReservationsFullInfo")]
        [ResponseType(typeof(List<Reservations>))]
        public HttpResponseMessage ReservationsFullInfo(int id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;
            
            var r= RBL.ReservationsFullInfo(id, Convert.ToInt32(UserID));

            if (r.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Reservations/ReservationDetails")]
        [ResponseType(typeof(List<Reservations>))]
        public HttpResponseMessage ReservationDetails(string id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;

            var r = RBL.Details(id);

            if (r.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Reservations/RemoveReservation")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage RemoveReservation(int id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = RBL.Remove(id, UserName);

            if (r.Length > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, true);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Reservations/RemoveFullReservation")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage RemoveFullReservation(string id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;

            var r = RBL.RemoveGUID(id, UserName);

            if (r)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Reservations/Index")]
        [ResponseType(typeof(List<ReservationLevel1>))]
        public HttpResponseMessage Index()
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;

            var r = RBL.ReservationsMainInfo(0, Convert.ToInt32(UserID));

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
            
        }

        [HttpPost]
        [Route("api/Reservations/Master")]
        [ResponseType(typeof(List<ReservationLevel1>))]
        public HttpResponseMessage Master()
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;

            var r = RBL.ReservationsMaster();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);

        }


    }
}