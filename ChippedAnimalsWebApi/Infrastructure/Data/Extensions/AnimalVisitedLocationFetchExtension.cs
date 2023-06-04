using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalVisitedLocationFetchExtension
    {
        public static async Task<AnimalVisitedLocation?> FetchByIdNoTrackingAsync(
            this IQueryable<AnimalVisitedLocation> query, long? visitedlocationId)
        {
            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == visitedlocationId);
        }
    }
}
