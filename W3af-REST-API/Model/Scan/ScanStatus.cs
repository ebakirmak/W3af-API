using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace W3af_REST_API.Model.Scan
{


    public partial class ScanStatus
    {
        //[JsonProperty("active_plugin")]
        //public ActivePlugin ActivePlugin { get; set; }

        [JsonProperty("current_request")]
        public CurrentRequest CurrentRequest { get; set; }

        //[JsonProperty("eta")]
        //public ActivePlugin Eta { get; set; }

        //[JsonProperty("exception")]
        //public object Exception { get; set; }

        //[JsonProperty("is_paused")]
        //public bool IsPaused { get; set; }

        [JsonProperty("is_running")]
        public bool IsRunning { get; set; }

        //[JsonProperty("queues")]
        //public ActivePlugin Queues { get; set; }

        [JsonProperty("rpm")]
        public long Rpm { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    //public partial class ActivePlugin
    //{
    //    [JsonProperty("audit")]
    //    public AuditUnion Audit { get; set; }

    //    [JsonProperty("crawl")]
    //    public AuditUnion Crawl { get; set; }
    //}

    public partial class CurrentRequest
    {
        [JsonProperty("audit")]
        public string Audit { get; set; }

        [JsonProperty("crawl")]
        public string Crawl { get; set; }
    }

    public partial class AuditClass
    {
        [JsonProperty("input_speed")]
        public double InputSpeed { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("output_speed")]
        public double OutputSpeed { get; set; }
    }

    public partial struct AuditUnion
    {
        public AuditClass AuditClass;
        public string String;
    }

    //public partial class ScanStatus
    //{
    //    public static ScanStatus FromJson(string json) => JsonConvert.DeserializeObject<ScanStatus>(json, W3af_REST_API.Model.Converter.Settings);
    //}

    //public partial struct AuditUnion
    //{
    //    public AuditUnion(JsonReader reader, JsonSerializer serializer)
    //    {
    //        AuditClass = null;
    //        String = null;

    //        switch (reader.TokenType)
    //        {
    //            case JsonToken.Null:
    //                return;
    //            case JsonToken.StartObject:
    //                AuditClass = serializer.Deserialize<AuditClass>(reader);
    //                return;
    //            case JsonToken.String:
    //            case JsonToken.Date:
    //                String = serializer.Deserialize<string>(reader);
    //                return;
    //        }
    //        throw new Exception("Cannot convert AuditUnion");
    //    }

    //    public void WriteJson(JsonWriter writer, JsonSerializer serializer)
    //    {
    //        if (AuditClass != null)
    //        {
    //            serializer.Serialize(writer, AuditClass);
    //            return;
    //        }
    //        if (String != null)
    //        {
    //            serializer.Serialize(writer, String);
    //            return;
    //        }
    //        writer.WriteNull();
    //    }
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this ScanStatus self) => JsonConvert.SerializeObject(self, W3af_REST_API.Model.Converter.Settings);
    //}

    //internal class Converter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(AuditUnion) || t == typeof(AuditUnion?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (t == typeof(AuditUnion) || t == typeof(AuditUnion?))
    //            return new AuditUnion(reader, serializer);
    //        throw new Exception("Unknown type");
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        var t = value.GetType();
    //        if (t == typeof(AuditUnion))
    //        {
    //            ((AuditUnion)value).WriteJson(writer, serializer);
    //            return;
    //        }
    //        throw new Exception("Unknown type");
    //    }

    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters = {
    //            new Converter(),
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}
}
