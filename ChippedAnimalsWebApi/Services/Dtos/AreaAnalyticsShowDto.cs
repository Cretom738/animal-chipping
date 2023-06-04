using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
    public record AreaAnalyticsShowDto
    {
        [RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$")]
        public string StartDate { get; init; } = null!;
        [RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$")]
        public string EndDate { get; init; } = null!;
    }
}
