using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("AccountRoles")]
    public class AccountRole
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Role { get; set; } = null!;
    }
}
