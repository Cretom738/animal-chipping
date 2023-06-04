using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AnimalAnalyticsDto
    {
        [JsonPropertyName("animalType")]
        public string AnimalType { get; set; } = null!;
        [JsonPropertyName("animalTypeId")]
        public long AnimalTypeId { get; set; }
        [JsonPropertyName("quantityAnimals")]
        public long AnimalQuantity { get; set; }
        [JsonPropertyName("animalsArrived")]
        public long ArrivedAnimalQuantity { get; set; }
        [JsonPropertyName("animalsGone")]
        public long GoneAnimalQuantity { get; set; }
    }
}
