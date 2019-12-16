using com.b_velop.Slipways.Web.Data.Dtos;
using System;

namespace com.b_velop.Slipways.Web.Data.Models
{
    public class Extra
    {
        public Extra()
        {

        }
        public Extra(
          ExtraDto e)
        {
            Id = e.Id;
            Name = Id.ToString().ToUpper() switch
            {
                "8976CEB5-19D6-4F5C-A34D-A43801667B40" => "parking-24.png",
                "F5836F04-E23B-475A-A079-1E4F3C9C4D87" => "camping-24.png",
                "06448FD8-DCC1-4579-947A-8A7B18BC1AAB" => "pier-24.png",
                _ => ""
            };
        }

        public Extra(
            Guid id)
        {
            Id = id;
            Name = id.ToString().ToUpper() switch
            {
                "8976CEB5-19D6-4F5C-A34D-A43801667B40" => "parking-24.png",
                "F5836F04-E23B-475A-A079-1E4F3C9C4D87" => "camping-24.png",
                "06448FD8-DCC1-4579-947A-8A7B18BC1AAB" => "pier-24.png",
                _ => ""
            };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
