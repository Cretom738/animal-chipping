using System.Text.Json.Serialization;
using Services.Dtos.Converters;

namespace Services.Dtos
{
    public record AnimalVisitedLocationDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("dateTimeOfVisitLocationPoint"), JsonConverter(typeof(UtcDateTimeConverter))]
        public DateTime VisitDateTime { get; set; }
        [JsonPropertyName("locationPointId")]
        public long LocationId { get; set; }
    }
}
