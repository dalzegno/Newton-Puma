using Logic.Enums;
using System.Collections.Generic;

namespace Logic.Models
{
    public class GradingDto
    {
        public int Id { get; set; }
        public GradeType GradeType { get; set; }
        public UserDto User { get; set; }
        public List<PoiGradingDto> PoiGradings { get; set; }
    }
}
