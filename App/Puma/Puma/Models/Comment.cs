﻿
namespace Puma.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int PointOfInterestId { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }

        //public PointOfInterestDto PointOfInterest { get; set; }
        //public User User { get; set; }
    }
}
