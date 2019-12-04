using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Slipway
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Stadt / Ort")]
        [Required]
        public string City { get; set; }

        [Range(-90.0, 90.0)]
        [Display(Name = "Breitengrad")]
        [Required]
        public double? Latitude { get; set; }

        [Range(-180.0, 180.0)]
        [Display(Name = "Längengrad")]
        [Required]
        public double? Longitude { get; set; }

        [Range(0, 500)]
        [Required]
        public decimal? Costs { get; set; }

        [Range(0, 5)]
        [Required]
        public int? Rating { get; set; }

        [Required]
        public string Water { get; set; }

        public string Street { get; set; }

        [StringLength(5)]
        public string Postalcode { get; set; }
    }
}
