using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AnimalTypeCreateDto
    {
        [JsonPropertyName("type")]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Type { get; init; } = null!;
    }
}
