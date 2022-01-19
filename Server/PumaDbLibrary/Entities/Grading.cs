﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary
{
    [Table("Grading")]
    public partial class Grading
    {
        public Grading()
        {
            PoiGradings = new HashSet<PoiGrading>();
        }

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "TINYINT")]
        public int GradeType { get; set; }
        [Column("User_Id")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Gradings")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(PoiGrading.Grading))]
        public virtual ICollection<PoiGrading> PoiGradings { get; set; }
    }
}