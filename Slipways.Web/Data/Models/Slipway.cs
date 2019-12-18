using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Slipway : IEntity
    {
        public Slipway()
        {
            //Extras = new HashSet<Extra>();
        }

        public Slipway(
            SlipwayDto slipway) : this()
        {
            Id = slipway.Id;
            City = slipway.City;
            Longitude = slipway.Longitude;
            Latitude = slipway.Latitude;
            Name = slipway.Name;
            Costs = slipway.Costs;
            Rating = slipway.Rating;
            Street = slipway.Street;
            Postalcode = slipway.Postalcode;
            Pro = slipway.Pro;
            Contra = slipway.Contra;
            Comment = slipway.Comment;
            Water = new Water(slipway.Water).Longname.FirstUpper();
            foreach (var extra in slipway.Extras)
            {
                Extras.Add(new Extra(extra));
            }
        }

        public Guid Id { get; set; }

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

        [Range(-1, 500)]
        [Required]
        [Display(Name = "Preis (unbekannt: -1)")]
        public decimal? Costs { get; set; }

        [Range(0, 5)]
        [Required]
        [Display(Name = "Bewertung")]
        public int? Rating { get; set; }

        [Required]
        [Display(Name = "Gewässer")]
        public string Water { get; set; }

        [Display(Name = "Straße")]
        public string Street { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        [Display(Name = "Pro")]
        public string Pro { get; set; }

        [Display(Name = "Kontra")]
        public string Contra { get; set; }

        [StringLength(5)]
        [Display(Name = "PLZ")]
        public string Postalcode { get; set; }

        public HashSet<Extra> Extras { get; set; }
    }
}
