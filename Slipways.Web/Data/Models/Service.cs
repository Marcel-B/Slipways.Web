using com.b_velop.Slipways.Web.Data.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Service
    {
        public Service()
        {
            Manufacturers = new HashSet<Manufacturer>();
        }

        public Service(
            ServiceDto s) : this()
        {
            Id = s.Id;
            Name = s.Name;
            Street = s.Street;
            Postalcode = s.Postalcode;
            City = s.City;
            Longitude = s.Longitude;
            Latitude = s.Latitude;
            Url = s.Url;
            Phone = s.Phone;
            Email = s.Email;

            if (s.Manufacturers == null)
                return;

            Manufacturers = new HashSet<Manufacturer>();

            foreach (var manufacturer in s.Manufacturers)
                Manufacturers.Add(new Manufacturer(manufacturer));
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Straße")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "PLZ")]
        public string Postalcode { get; set; }

        [Required]
        [Display(Name = "Stadt / Ort")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Längengrad")]
        public double Longitude { get; set; }

        [Required]
        [Display(Name = "Breitengrad")]
        public double Latitude { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public HashSet<Manufacturer> Manufacturers { get; set; }
    }
}
