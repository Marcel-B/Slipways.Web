using com.b_velop.Slipways.Web.Data.Models;
using System;

namespace com.b_velop.Slipways.Web.Data.Dtos
{
    public class ExtraDto
    {
        public ExtraDto()
        {

        }

        public ExtraDto(
            Extra e)
        {
            Id = e.Id;
            Name = e.Name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
