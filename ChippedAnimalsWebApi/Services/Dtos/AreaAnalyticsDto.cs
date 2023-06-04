using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AreaAnalyticsDto
    {
        [JsonPropertyName("totalQuantityAnimals")]
        public long TotalAnimalQuantity { get; set; }
        [JsonPropertyName("totalAnimalsArrived")]
        public long TotalArrivedAnimalQuantity { get; set; }
        [JsonPropertyName("totalAnimalsGone")]
        public long TotalGoneAnimalQuantity { get; set; }
        [JsonPropertyName("animalsAnalytics")]
        public ICollection<AnimalAnalyticsDto> AnimalAnalytics { get; set; } = null!;
    }
}
