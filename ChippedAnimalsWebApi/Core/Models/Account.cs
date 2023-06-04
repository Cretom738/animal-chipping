using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Destructurama.Attributed;

namespace Core.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string FirstName { get; set; } = null!;
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string LastName { get; set; } = null!;
        [Required(AllowEmptyStrings = false), EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;
        [NotLogged]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Password { get; set; } = null!;
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public AccountRole Role { get; set; } = null!;
    }
}
