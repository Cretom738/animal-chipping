using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalTypeFetchExtension
    {
        public static async Task<ICollection<AnimalType>> FetchByIdsAsync(
            this IQueryable<AnimalType> query, ICollection<long> typeIds)
        {
            return await query
                .Where(at => typeIds.Contains(at.Id))
                .ToListAsync();
        }

        public static async Task<AnimalType?> FetchByIdNoTrackingAsync(
            this IQueryable<AnimalType> query, long? typeId)
        {
            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == typeId);
        }

        public static async Task<IList<AnimalType>> FetchListForAnalyticsAsync(
            this DbSet<AnimalType> query)
        {
            return await query
                .Include(at => at.Animals)
                    .ThenInclude(a => a.ChippingLocation)
                .Include(at => at.Animals)
                    .ThenInclude(a => a.VisitedLocations)
                        .ThenInclude(avl => avl.Location)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
