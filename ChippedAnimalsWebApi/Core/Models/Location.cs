using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    [Table("Locations")]
    public class Location
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Range(-90D, 90D)]
        public double Latitude { get; set; }
        [Range(-180D, 180D)]
        public double Longitude { get; set; }
    }
}
