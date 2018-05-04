using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace W3af_REST_API.Model.Scan
{

    public class Scan
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("errors")]
        public bool Errors { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("target_urls")]
        public List<string> TargetUrls { get; set; }
    }
}
