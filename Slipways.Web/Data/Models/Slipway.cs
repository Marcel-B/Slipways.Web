using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Slipway
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Stadt / Ort")]
        public string City { get; set; }

        [Range(-90.0, 90.0)]
        [Display(Name = "Breitengrad")]
        public double? Latitude { get; set; }

        [Range(-180.0, 180.0)]
        [Display(Name = "Längengrad")]
        public double? Longitude { get; set; }

        [Range(0, 500)]
        public decimal? Costs { get; set; }

        [Range(0, 5)]
        public int? Rating { get; set; }

        public string Water { get; set; }
    }
}
