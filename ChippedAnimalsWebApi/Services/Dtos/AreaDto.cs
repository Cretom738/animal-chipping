using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AreaDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        [JsonPropertyName("areaPoints")]
        public ICollection<AreaPointDto> AreaPoints { get; set; } = null!;
    }
}
