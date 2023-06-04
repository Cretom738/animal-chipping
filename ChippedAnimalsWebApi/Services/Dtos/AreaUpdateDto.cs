using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AreaUpdateDto
    {
        [JsonPropertyName("name")]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Name { get; set; } = null!;
        [JsonPropertyName("areaPoints")]
        [MinLength(3)]
        public ICollection<AreaPointUpdateDto> AreaPoints { get; set; } = null!;
    }
}
