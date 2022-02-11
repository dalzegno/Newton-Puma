using Puma.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Puma.Models
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PositionPoi Position { get; set; }
        public Address Address { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Grading> Gradings { get; set; }
        public List<PoiTag> PoiTags { get; set; }

        public int LikeCounter
        {
            get
            {
                if (Gradings == null || Gradings.Count() < 1) 
                    return 0;

                return Gradings.Where(g => g.GradeType == GradeType.Liked).Count();
            }
        }
    }
}
