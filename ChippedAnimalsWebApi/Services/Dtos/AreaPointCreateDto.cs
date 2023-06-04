using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AreaPointCreateDto
    {
        [JsonPropertyName("latitude")]
        [Required, Range(-90D, 90D)]
        public double? Latitude { get; init; }
        [JsonPropertyName("longitude")]
        [Required, Range(-180D, 180D)]
        public double? Longitude { get; init; }
    }
}
