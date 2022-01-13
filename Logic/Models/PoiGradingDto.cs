using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class PoiGradingDto
    {
        public int Id { get; set; }
        public GradingDto Grading { get; set; }
        public PointOfInterestDto PointOfInterest { get; set; }
    }
}
