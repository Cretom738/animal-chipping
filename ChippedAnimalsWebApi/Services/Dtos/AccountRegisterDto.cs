using Destructurama.Attributed;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AccountRegistrationDto
    {
        [JsonPropertyName("firstName")]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string FirstName { get; init; } = null!;
        [JsonPropertyName("lastName")]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string LastName { get; init; } = null!;
        [JsonPropertyName("email")]
        [Required(AllowEmptyStrings = false), EmailAddress, MaxLength(255)]
        public string Email { get; init; } = null!;
        [NotLogged]
        [JsonPropertyName("password")]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Password { get; init; } = null!;
    }
}
