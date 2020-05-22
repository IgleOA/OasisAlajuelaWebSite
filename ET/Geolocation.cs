using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ET
{
    public class Geolocation
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("as")]
        public As As { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }
    }

    public class As
    {
        [JsonProperty("asn")]
        public long Asn { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonProperty("domain")]
        public Uri Domain { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Location
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("geonameId")]
        public long GeonameId { get; set; }
    }
}
