using Logic.Enums;

namespace Logic.Models
{
    public class GradingDto
    {
        public int Id { get; set; }
        public GradeType GradeType { get; set; }
        public int UserId { get; set; }
        public int PointOfInterestId { get; set; }
        //public UserDto User { get; set; }
        //public PointOfInterestDto PointOfInterest { get; set; }
    }
}
