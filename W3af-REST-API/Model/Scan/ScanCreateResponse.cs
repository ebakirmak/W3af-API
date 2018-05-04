using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace W3af_REST_API.Model.Scan
{


    public class ScanCreateResponse
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

 
}
