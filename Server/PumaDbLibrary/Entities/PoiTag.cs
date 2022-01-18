using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary
{
    [Table("Poi_Tag")]
    public partial class PoiTag
    {
        [Key]
        public int Id { get; set; }
        [Column("Tag_Id")]
        public int TagId { get; set; }
        [Column("Point_Of_Interest_Id")]
        public int PointOfInterestId { get; set; }

        [ForeignKey(nameof(PointOfInterestId))]
        [InverseProperty("PoiTags")]
        public virtual PointOfInterest PointOfInterest { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("PoiTags")]
        public virtual Tag Tag { get; set; }
    }
}
