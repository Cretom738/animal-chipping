using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
    public record AccountListDto
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        [Range(0, int.MaxValue)]
        public int? From { get; init; }
        [Range(1, int.MaxValue)]
        public int? Size { get; init; }
    }
}
