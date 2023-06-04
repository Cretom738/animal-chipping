using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AreaPointDto
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
