using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AnimalAnimalTypeUpdateDto
    {
        [JsonPropertyName("oldTypeId")]
        [Required, Range(1, long.MaxValue)]
        public long? OldTypeId { get; init; }
        [JsonPropertyName("newTypeId")]
        [Required, Range(1, long.MaxValue)]
        public long? NewTypeId { get; init; }
    }
}
