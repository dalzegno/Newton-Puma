using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary.Entities
{
    [Table("Position")]
    public partial class Position
    {
        public Position()
        {
            PointOfInterests = new HashSet<PointOfInterest>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "DOUBLE")]
        public double Latitude { get; set; }
        [Required]
        [Column(TypeName = "DOUBLE")]
        public double Longitude { get; set; }

        [InverseProperty(nameof(PointOfInterest.Position))]
        public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
    }
}
