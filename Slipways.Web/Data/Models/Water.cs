using System;
using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Water
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Longname { get; set; }

        [Display(Name = "Kurzbezeichnung")]
        [Required]
        public string Shortname { get; set; }
    }
}
