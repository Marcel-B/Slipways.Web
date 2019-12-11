using System;
using System.Text.Json.Serialization;

namespace com.b_velop.Slipways.Web.Data.Dtos
{
    public class WaterDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("longname")]
        public string Longname { get; set; }

        [JsonPropertyName("shortname")]
        public string Shortname { get; set; }
    }
}
