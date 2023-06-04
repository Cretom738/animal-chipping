using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    [Table("Animals")]
    public class Animal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Range(float.Epsilon, float.MaxValue)]
        public float Weight { get; set; }
        [Range(float.Epsilon, float.MaxValue)]
        public float Length { get; set; }
        [Range(float.Epsilon, float.MaxValue)]
        public float Height { get; set; }
        public DateTime ChippingDateTime { get; set; }
        public DateTime? DeathDateTime { get; set; }
        public AnimalGender Gender { get; set; } = null!;
        public AnimalLifeStatus LifeStatus { get; set; } = null!;
        [Range(1, int.MaxValue)]
        public int ChipperId { get; set; }
        [ForeignKey(nameof(ChipperId))]
        public Account Chipper { get; set; } = null!;
        [Range(1, long.MaxValue)]
        public long ChippingLocationId { get; set; }
        [ForeignKey(nameof(ChippingLocationId))]
        public Location ChippingLocation { get; set; } = null!;
        public ICollection<AnimalType> Types { get; set; } 
            = new List<AnimalType>();
        public ICollection<AnimalVisitedLocation> VisitedLocations { get; set; } 
            = new List<AnimalVisitedLocation>();

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            Animal animal = (Animal)obj;
            if (animal.Id != Id) return false;
            return true;
        }
    }
}
