using System;
using System.Collections.Generic;

namespace com.b_velop.Slipways.Web.Data.Dtos
{
    public class SlipwayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public decimal Costs { get; set; }
        public int Rating { get; set; }
        public string Street { get; set; }
        public string Postalcode { get; set; }
        public Guid Water { get; set; }
        public IEnumerable<Guid> Extras { get; set; }
    }
}
