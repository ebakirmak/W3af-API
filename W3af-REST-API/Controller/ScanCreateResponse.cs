// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using W3af-API;
//
//    var scanCreateResponse = ScanCreateResponse.FromJson(jsonString);

namespace W3af_REST_API.Controller
{
    using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public  class ScanCreateResponse
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
