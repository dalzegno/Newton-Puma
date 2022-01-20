
namespace Logic.Models
{
    public class AddCommentDto
    {
        public string Body { get; set; }
        public int PointOfInterestId { get; set; }
        public int UserId { get; set; }
    }
}
