using com.b_velop.Slipways.Web.Data.Models;
using System;
using System.Text.Json.Serialization;

namespace com.b_velop.Slipways.Web.Data.Dtos
{
    public class ManufacturerDto
    {
        public ManufacturerDto() { }

        public ManufacturerDto(
            Manufacturer m)
        {
            Id = m.Id;
            Name = m.Name;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
