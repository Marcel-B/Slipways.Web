using com.b_velop.Slipways.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace com.b_velop.Slipways.Web.Data.Dtos
{
    public class SlipwayDto : IEntity
    {
        public SlipwayDto()
        {
            Extras = new List<ExtraDto>();
        }

        public SlipwayDto(
            Slipway s) : this()
        {
            Id = s.Id;
            City = s.City;
            Comment = s.Comment;
            Contra = s.Contra;
            Costs = s.Costs.Value;

            if (s.Extras != null)
                foreach (var extra in s.Extras)
                    ((List<ExtraDto>)Extras).Add(new ExtraDto(extra));


            Latitude = s.Latitude.Value;
            Longitude = s.Longitude.Value;
            Name = s.Name;
            Postalcode = s.Postalcode;
            Pro = s.Pro;
            Rating = s.Rating ?? 0;
            Street = s.Street;
            Water = new WaterDto(s.Water);
        }

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
