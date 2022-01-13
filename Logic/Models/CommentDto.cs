using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public PointOfInterestDto PointOfInterest { get; set; }
        public UserDto User { get; set; }
    }
}
