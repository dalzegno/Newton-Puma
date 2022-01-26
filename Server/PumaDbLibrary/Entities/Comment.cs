using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary.Entities
{
    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(400)")]
        public string Body { get; set; }
        [Column("Point_Of_Interest_Id")]
        public int PointOfInterestId { get; set; }
        [Column("User_Id")]
        public int UserId { get; set; }

        [ForeignKey(nameof(PointOfInterestId))]
        [InverseProperty("Comments")]
        public virtual PointOfInterest PointOfInterest { get; set; }
        
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Comments")]
        public virtual User User { get; set; }
    }
}
