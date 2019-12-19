using com.b_velop.Slipways.Web.Data.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Manufacturer : IEntity
    {
        public Manufacturer() { }

        public Manufacturer(
            ManufacturerDto m)
        {
            Id = m.Id;
            Name = m.Name;
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
