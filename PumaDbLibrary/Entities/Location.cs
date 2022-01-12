using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary
{
    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            PointOfInterests = new HashSet<PointOfInterest>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL")]
        public byte[] Latitude { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL")]
        public byte[] Longitude { get; set; }
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }

        [InverseProperty(nameof(PointOfInterest.Location))]
        public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
    }
}
