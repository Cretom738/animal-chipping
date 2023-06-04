using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AreaFetchExtension
    {
        public static async Task<Area?> FetchByIdAsync(
            this IQueryable<Area> query, long? areaId)
        {
            return await query
                .Include(a => a.AreaPoints)
                .FirstOrDefaultAsync(a => a.Id == areaId);
        }

        public static async Task<Area?> FetchByIdNoTrackingAsync(
            this IQueryable<Area> query, long? areaId)
        {
            return await query
                .Include(a => a.AreaPoints)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == areaId);
        }

        public static async Task<IList<Area>> FetchAllNoTrackingOrderedPointsAsync(
            this IQueryable<Area> query)
        {
            return await query
                .Include(a => a.AreaPoints.OrderBy(ap => ap.Id))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
