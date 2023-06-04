using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class LocationCheckExtension
    {
        public static async Task<bool> DoCoordinatesExistAsync(
            this IQueryable<Location> query, double? latitude, double? longitude)
        {
            return await query.AnyAsync(l =>
                l.Latitude == latitude
                && l.Longitude == longitude);
        }

        public static async Task<bool> DoesExistsAsync(
            this IQueryable<Location> query, long? locationId)
        {
            return await query.AnyAsync(l => l.Id == locationId);
        }
    }
}
