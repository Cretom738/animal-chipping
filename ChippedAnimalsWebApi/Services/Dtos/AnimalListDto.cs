using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
    public record AnimalListDto
    {
        public DateTime? StartDateTime { get; init; }
        public DateTime? EndDateTime { get; init; }
        public string? LifeStatus { get; init; }
        public string? Gender { get; init; }
        [Range(1, int.MaxValue)]
        public int? ChipperId { get; init; }
        [Range(1, long.MaxValue)]
        public long? ChippingLocationId { get; init; }
        [Range(0, int.MaxValue)]
        public int? From { get; init; }
        [Range(1, int.MaxValue)]
        public int? Size { get; init; }
    }
}
