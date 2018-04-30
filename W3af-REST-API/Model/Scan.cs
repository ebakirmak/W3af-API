using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace W3af_REST_API.Model
{
    public class Scan
    {
        private string Target_URL;

        public string TargetURL
        {
            get { return Target_URL; }
            set { Target_URL = value; }
        }

        private string Scan_Profile;

        public string ScanProfile
        {
            get { return Scan_Profile; }
            set { Scan_Profile = value; }
        }


        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }


        public Scan()
        {

        }

        



    }
}
