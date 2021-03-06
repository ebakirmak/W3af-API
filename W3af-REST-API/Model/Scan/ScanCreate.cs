﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W3af_REST_API.Model.Scan
{
    public class ScanCreate
    {
        [JsonProperty("scan_profile")]
        public string Scan_Profile { get; set; }

        [JsonProperty("target_urls")]
        public string[] Target_URL { get; set; }

        public ScanCreate( string scanProfile, string targetUrl )
        {
            this.Scan_Profile = scanProfile;
            this.Target_URL = new string[1];
            this.Target_URL[0] = targetUrl;
        }
    }
}
