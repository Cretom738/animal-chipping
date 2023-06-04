using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AnimalVisitedLocationUpdateDto
    {
        [JsonPropertyName("visitedLocationPointId")]
        [Required, Range(1, long.MaxValue)]
        public long? VisitedLocationId { get; init; }
        [JsonPropertyName("locationPointId")]
        [Required, Range(1, long.MaxValue)]
        public long? LocationId { get; init; }
    }
}
