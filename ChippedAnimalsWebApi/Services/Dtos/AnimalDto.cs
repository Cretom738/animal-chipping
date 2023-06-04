using System.Text.Json.Serialization;
using Services.Dtos.Converters;

namespace Services.Dtos
{
    public record AnimalDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("weight")]
        public float Weight { get; set; }
        [JsonPropertyName("length")]
        public float Length { get; set; }
        [JsonPropertyName("height")]
        public float Height { get; set; }
        [JsonPropertyName("gender")]
        public string Gender { get; set; } = null!;
        [JsonPropertyName("lifeStatus")]
        public string LifeStatus { get; set; } = null!;
        [JsonPropertyName("chippingDateTime"), JsonConverter(typeof(UtcDateTimeConverter))]
        public DateTime ChippingDateTime { get; set; }
        [JsonPropertyName("deathDateTime"), JsonConverter(typeof(UtcDateTimeConverter))]
        public DateTime? DeathDateTime { get; set; }
        [JsonPropertyName("chipperId")]
        public int ChipperId { get; set; }
        [JsonPropertyName("chippingLocationId")]
        public long ChippingLocationId { get; set; }
        [JsonPropertyName("visitedLocations")]
        public ICollection<long> VisitedLocations { get; set; } = null!;
        [JsonPropertyName("animalTypes")]
        public ICollection<long> Types { get; set; } = null!;
    }
}
