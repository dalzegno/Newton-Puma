using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PumaDbLibrary.Entities
{
    [Table("Api")]
    public class Api
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
