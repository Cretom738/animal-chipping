using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AnimalTypeDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }
}
