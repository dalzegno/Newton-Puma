using Puma.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puma.Models
{
    public class AddGradeDto
    {
        public int PoiId { get; set; }
        public int UserId { get; set; }
        public int Grade { get; set; }
    }
}
