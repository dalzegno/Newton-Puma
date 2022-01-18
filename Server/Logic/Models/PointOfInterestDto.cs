using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }
        public LocationDto Location { get; set; }
        public List<CommentDto> Comments{ get; set; }
        public List<PoiGradingDto> PoiGradings { get; set; }
        public virtual List<PoiTagDto> PoiTags { get; set; }
    }
}
