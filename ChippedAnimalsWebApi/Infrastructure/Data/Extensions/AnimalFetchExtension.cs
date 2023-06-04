using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalFetchExtension
    {
        public static async Task<Animal?> FetchByIdAsync(
            this IQueryable<Animal> query, long? animalId)
        {
            return await query
                .Include(a => a.LifeStatus)
                .Include(a => a.Gender)
                .Include(a => a.VisitedLocations)
                .Include(a => a.Types)
                .FirstOrDefaultAsync(a => a.Id == animalId);
        }

        public static async Task<Animal?> FetchByIdNoTrackingAsync(
            this IQueryable<Animal> query, long? animalId)
        {
            return await query
                .Include(a => a.LifeStatus)
                .Include(a => a.Gender)
                .Include(a => a.VisitedLocations)
                .Include(a => a.Types)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == animalId);
        }

        public static async Task<IList<Animal>> FetchListAsync(
            this IQueryable<Animal> query)
        {
            return await query
                .Include(a => a.LifeStatus)
                .Include(a => a.Gender)
                .Include(a => a.VisitedLocations)
                .Include(a => a.Types)
                .ToListAsync();
        }
    }
}
