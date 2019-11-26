using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Slipway
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Stadt / Ort")]
        public string City { get; set; }

        [Display(Name = "Längengrad")]
        public double? Latitude { get; set; }

        [Display(Name = "Breitengrad")]
        public double? Longitude { get; set; }

        public decimal? Costs { get; set; }

        public int Rating { get; set; }

        public string Water { get; set; }
    }
}
