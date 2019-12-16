using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace com.b_velop.Slipways.Web.Data.Dtos
{
    public class SlipwayDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime? Updated { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("waterFk")]
        public Guid WaterFk { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("postalcode")]
        public string Postalcode { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("costs")]
        public decimal Costs { get; set; }

        [JsonPropertyName("pro")]
        public string Pro { get; set; }

        [JsonPropertyName("contra")]
        public string Contra { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("extras")]
        public IEnumerable<ExtraDto> Extras { get; set; }

        [JsonPropertyName("water")]
        public WaterDto Water { get; set; }
    }
}
