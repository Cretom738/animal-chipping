using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record ErrorDto
    {
        [JsonPropertyName("error")]
        public string Error { get; set; } = null!;
    }
}
