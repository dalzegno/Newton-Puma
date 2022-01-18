using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary
{
    [Table("Point_Of_Interest")]
    public partial class PointOfInterest
    {
        public PointOfInterest()
        {
            Comments = new HashSet<Comment>();
            PoiGradings = new HashSet<PoiGrading>();
            PoiTags = new HashSet<PoiTag>();
        }

        [Key]
        public int Id { get; set; }
        [Column("Location_Id")]
        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("PointOfInterests")]
        public virtual Location Location { get; set; }
        [InverseProperty(nameof(Comment.PointOfInterest))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(PoiGrading.PointOfInterest))]
        public virtual ICollection<PoiGrading> PoiGradings { get; set; }
        [InverseProperty(nameof(PoiTag.PointOfInterest))]
        public virtual ICollection<PoiTag> PoiTags { get; set; }
    }
}
