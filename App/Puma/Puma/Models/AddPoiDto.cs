using System.Collections.Generic;

namespace Puma.Models
{
    public class AddPoiDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PositionPoi Position { get; set; }
        public Address Address { get; set; }
        public List<int> TagIds { get; set; }
    }
}

