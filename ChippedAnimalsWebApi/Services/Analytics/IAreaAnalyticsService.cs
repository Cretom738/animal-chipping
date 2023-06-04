using Services.Dtos;

namespace Services.Analytics
{
    public interface IAreaAnalyticsService
    {
        public Task<AreaAnalyticsDto> GetAreaAnalyticsAsync(long? areaId, AreaAnalyticsShowDto interval);
    }
}
