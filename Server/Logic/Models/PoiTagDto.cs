using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class PoiTagDto
    {
        public int Id { get; set; }
        public PointOfInterestDto PointOfInterest { get; set; }
        public TagDto Tag { get; set; }
    }
}
