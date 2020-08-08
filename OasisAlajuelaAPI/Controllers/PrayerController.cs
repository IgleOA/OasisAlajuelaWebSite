using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL;
using ET;
using Newtonsoft.Json;

namespace OasisAlajuelaAPI.Controllers
{
    public class PrayerController : ApiController
    {
        private PrayersBL PBL = new PrayersBL();
        private static string API_KEY = ConfigurationManager.AppSettings["APIStack_KEY"].ToString();
        private static string API_URL = ConfigurationManager.AppSettings["APIStack_URL"].ToString();

        [HttpGet]
        [Route("api/Prayer/New")]
        public HttpResponseMessage AddNew([FromBody] Prayers Detail)
        {
            GeolocationStack location = GetGeolocation(Detail.IP);

            Detail.Country = location.CountryName;
            Detail.Region = location.RegionName;
            Detail.City = location.City;

            var r = PBL.Add(Detail);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK);
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
    }
}