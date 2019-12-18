using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Water : IEntity
    {
        public Water() { }

        public Water(
            WaterDto w)
        {
            Id = w.Id;
            Longname = w.Longname.FirstUpper();
            Shortname = w.Shortname;
        }

        public Guid Id { get; set; }

        private string _longname;

        [Display(Name = "Name")]
        [Required]
        public string Longname { get => _longname.FirstUpper();  set => _longname = value; }

        [Display(Name = "Kurzbezeichnung")]
        [Required]
        public string Shortname { get; set; }
    }
}
