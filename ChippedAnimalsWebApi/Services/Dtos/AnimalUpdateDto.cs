using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AnimalUpdateDto
    {
        [JsonPropertyName("weight")]
        [Required, Range(float.Epsilon, float.MaxValue)]
        public float? Weight { get; init; }
        [JsonPropertyName("length")]
        [Required, Range(float.Epsilon, float.MaxValue)]
        public float? Length { get; init; }
        [JsonPropertyName("height")]
        [Required, Range(float.Epsilon, float.MaxValue)]
        public float? Height { get; init; }
        [JsonPropertyName("gender")]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Gender { get; init; } = null!;
        [JsonPropertyName("lifeStatus")]
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string LifeStatus { get; init; } = null!;
        [JsonPropertyName("chipperId")]
        [Required, Range(1, int.MaxValue)]
        public int? ChipperId { get; init; }
        [JsonPropertyName("chippingLocationId")]
        [Required, Range(1, long.MaxValue)]
        public long? ChippingLocationId { get; init; }
    }
}
