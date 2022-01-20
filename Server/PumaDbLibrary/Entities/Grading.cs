using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary.Entities
{
    [Table("Grading")]
    public partial class Grading
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "TINYINT")]
        public int GradeType { get; set; }
        
        [Column("User_Id")]
        public int UserId { get; set; }

        [Column("Point_Of_Interest_Id")]
        public int PointOfInterestId { get; set; } 

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Gradings")]
        public virtual User User { get; set; }

        [ForeignKey(nameof(PointOfInterestId))]
        [InverseProperty("Gradings")]
        public virtual PointOfInterest PointOfInterest { get; set; }
    }
}
