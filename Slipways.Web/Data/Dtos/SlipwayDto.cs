using System;
namespace com.b_velop.Slipways.Web.Data.Dtos
{
    public class SlipwayDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid Water { get; set; }
    }
}
