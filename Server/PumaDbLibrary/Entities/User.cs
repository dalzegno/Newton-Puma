using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PumaDbLibrary
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Gradings = new HashSet<Grading>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(45)")]
        public string Password { get; set; }
        [Column(TypeName = "nvarchar(45)")]
        public string DisplayName { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(60)")]
        public string LastName { get; set; }
        [Column(TypeName = "integer")]
        public bool IsAdmin { get; set; }
        [Column(TypeName = "integer")]
        public bool IsSuperAdmin { get; set; }

        [InverseProperty(nameof(Comment.User))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(Grading.User))]
        public virtual ICollection<Grading> Gradings { get; set; }
    }
}
