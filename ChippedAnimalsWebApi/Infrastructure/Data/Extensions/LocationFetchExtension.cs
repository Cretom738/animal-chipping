using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class LocationFetchExtension
    {
        public static async Task<Location?> FetchByIdNoTrackingAsync(
            this IQueryable<Location> query, long? pointId)
        {
            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == pointId);
        }

        public static async Task<long> FetchIdByCoordinatesAsync(
            this IQueryable<Location> query, double? latitude, double? longitude)
        {
            return await query
                .Where(l => l.Latitude == latitude 
                    && l.Longitude == longitude)
                .Select(l => l.Id)
                .FirstOrDefaultAsync();
        }
    }
}
