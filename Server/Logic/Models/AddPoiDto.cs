using System.Collections.Generic;

namespace Logic.Models
{
    public class AddPoiDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PositionDto Position { get; set; }
        public AddressDto Address { get; set; }
        public List<int> TagIds { get; set; }
    }
}

