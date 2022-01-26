
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PumaDbLibrary.Entities
{
    [Table("Address")]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string StreetName { get; set; }

        [InverseProperty(nameof(PointOfInterest.Address))]
        public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
    }
}
