using Destructurama.Attributed;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    [Table("AreaPoints")]
    public class AreaPoint
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Range(-90D, 90D)]
        public double Latitude { get; set; }
        [Range(-180D, 180D)]
        public double Longitude { get; set; }
        [Range(1, long.MaxValue)]
        public long AreaId { get; set; }
        [NotLogged]
        [ForeignKey(nameof(AreaId))]
        public Area Area { get; set; } = null!;

        public override int GetHashCode()
        {
            int hash = Latitude.GetHashCode();
            hash = 31 * hash + Longitude.GetHashCode();
            return hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            AreaPoint areaPoint = (AreaPoint)obj;
            if (Latitude != areaPoint.Latitude) return false;
            if (Longitude != areaPoint.Longitude) return false;
            return true;
        }
    }
}
