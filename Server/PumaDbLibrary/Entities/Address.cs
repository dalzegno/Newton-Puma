using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumaDbLibrary.Entities
{
    [Table("Address")]
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string ZipCode { get; set; }

        [InverseProperty(nameof(PointOfInterest.Address))]
        public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
    }
}
