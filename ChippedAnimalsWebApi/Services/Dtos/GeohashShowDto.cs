using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
    public record GeohashShowDto
    {
        [Required, Range(-90D, 90D)]
        public double? Latitude { get; init; }
        [Required, Range(-180D, 180D)]
        public double? Longitude { get; init; }
    }
}
