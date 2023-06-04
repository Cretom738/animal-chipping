using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Destructurama.Attributed;

namespace Core.Models
{
    [Table("AnimalVisitedLocations")]
    public class AnimalVisitedLocation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime VisitDateTime { get; set; }
        [Range(1, long.MaxValue)]
        public long LocationId { get; set; }
        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; } = null!;
        [Range(1, long.MaxValue)]
        public long AnimalId { get; set; }
        [NotLogged]
        [ForeignKey(nameof(AnimalId))]
        public Animal Animal { get; set; } = null!;

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            AnimalVisitedLocation visitedLocation = (AnimalVisitedLocation) obj;
            if (visitedLocation.Id != Id) return false;
            return true;
        }
    }
}
