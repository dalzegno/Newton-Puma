using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary
{
    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            PoiTags = new HashSet<PoiTag>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(45)")]
        public string Name { get; set; }

        [InverseProperty(nameof(PoiTag.Tag))]
        public virtual ICollection<PoiTag> PoiTags { get; set; }
    }
}
