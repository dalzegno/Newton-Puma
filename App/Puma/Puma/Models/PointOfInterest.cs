using System.Collections.Generic;

namespace Puma.Models
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PositionPoi Position { get; set; }
        public Address Address { get; set; }
        public List<Comment> Comments{ get; set; }
        public List<Grading> Gradings { get; set; }
        public List<PoiTag> PoiTags { get; set; }
    }
}
