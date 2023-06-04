using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Destructurama.Attributed;

namespace Core.Models
{
    [Table("AnimalTypes")]
    public class AnimalType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Type { get; set; } = null!;
        [NotLogged]
        public ICollection<Animal> Animals { get; set; } = new List<Animal>();

        public override int GetHashCode()
        {
            int hash = Id.GetHashCode();
            return 31 * hash + Type.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            AnimalType type = (AnimalType) obj;
            return type.Id == Id;
        }
    }
}
