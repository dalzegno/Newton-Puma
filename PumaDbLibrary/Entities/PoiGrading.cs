using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary
{
    [Table("Poi_Grading")]
    public partial class PoiGrading
    {
        [Key]
        public long Id { get; set; }
        [Column("Grading_Id")]
        public long GradingId { get; set; }
        [Column("Point_Of_Interest_Id")]
        public long PointOfInterestId { get; set; }

        [ForeignKey(nameof(GradingId))]
        [InverseProperty("PoiGradings")]
        public virtual Grading Grading { get; set; }
        [ForeignKey(nameof(PointOfInterestId))]
        [InverseProperty("PoiGradings")]
        public virtual PointOfInterest PointOfInterest { get; set; }
    }
}
