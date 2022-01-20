using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PumaDbLibrary.Entities;

#nullable disable

namespace PumaDbLibrary.Entities
{
    [Table("Point_Of_Interest")]
    public partial class PointOfInterest
    {
        public PointOfInterest()
        {
            Comments = new HashSet<Comment>();
            Gradings = new HashSet<Grading>();
            PoiTags = new HashSet<PoiTag>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column("Name")]
        public string Name { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Position_Id")]
        public int PositionId { get; set; }
        [Column("Address_Id")]
        public int AddressId { get; set; }


        [ForeignKey(nameof(PositionId))]
        [InverseProperty("PointOfInterests")]
        public virtual Position Position { get; set; }

        [ForeignKey(nameof(AddressId))]
        [InverseProperty("PointOfInterests")]
        public virtual Address Address { get; set; }

        [InverseProperty(nameof(Comment.PointOfInterest))]
        public virtual ICollection<Comment> Comments { get; set; }
        
        [InverseProperty(nameof(Grading.PointOfInterest))]
        public virtual ICollection<Grading> Gradings { get; set; }
        
        [InverseProperty(nameof(PoiTag.PointOfInterest))]
        public virtual ICollection<PoiTag> PoiTags { get; set; }
    }
}
