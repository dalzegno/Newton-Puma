using System.Collections.Generic;

namespace Logic.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PositionDto Position { get; set; }
        public AddressDto Address { get; set; }
        public List<CommentDto> Comments{ get; set; }
        public List<GradingDto> Gradings { get; set; }
        public List<PoiTagDto> PoiTags { get; set; }
    }
}
