using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
    public class AnimalVisitedLocationListDto
    {
        public DateTime? StartDateTime { get; init; }
        public DateTime? EndDateTime { get; init; }
        [Range(0, int.MaxValue)]
        public int? From { get; init; }
        [Range(1, int.MaxValue)]
        public int? Size { get; init; }
    }
}
