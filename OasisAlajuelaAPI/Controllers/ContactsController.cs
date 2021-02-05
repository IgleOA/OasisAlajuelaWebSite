using BL;
using ET;
using Newtonsoft.Json;
using OasisAlajuelaAPI.Filters;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace OasisAlajuelaAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContactsController : ApiController
    {
        private ContactsBL CBL = new ContactsBL();
        private static string API_KEY = ConfigurationManager.AppSettings["APIStack_KEY"].ToString();
        private static string API_URL = ConfigurationManager.AppSettings["APIStack_URL"].ToString();

        [HttpPost]
        [Route("api/Contact/New")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Contacts Detail)
        {
            GeolocationStack location = GetGeolocation(Detail.IP);

            Detail.Country = location.CountryName;
            Detail.Region = location.RegionName;
            Detail.City = location.City;

            var r = CBL.Add(Detail);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }            
        }
        static GeolocationStack GetGeolocation(string IP)
        {

            string url = API_URL + IP + $"?access_key={API_KEY}";
            string resultData = string.Empty;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resultData = reader.ReadToEnd();
            }

            GeolocationStack location = JsonConvert.DeserializeObject<GeolocationStack>(resultData);

            return location;
        }

        [HttpPost]
        [Route("api/Contact")]
        [ApiKeyAuthentication]
        [ResponseType(typeof(ContactType))]
        public HttpResponseMessage List([FromBody] ContactListRequest model)
        {
            var CT = CBL.Details(model.ContactTypeID);

            CT.ContactList = CBL.ContactList(model);

            if (CT.ContactTypeID > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, CT);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }

        [HttpPost]
        [Route("api/Contact/ContactTypes")]
        [ResponseType(typeof(List<ContactType>))]
        public HttpResponseMessage ContactTypeList()
        {
            var r = CBL.ContactTypesList();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);           
        }

    }
}