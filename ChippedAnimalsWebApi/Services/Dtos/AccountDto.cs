using System.Text.Json.Serialization;

namespace Services.Dtos
{
    public record AccountDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = null!;
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = null!;
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;
        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;
    }
}
