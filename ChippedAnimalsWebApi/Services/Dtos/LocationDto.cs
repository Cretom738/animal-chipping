using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record LocationDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
