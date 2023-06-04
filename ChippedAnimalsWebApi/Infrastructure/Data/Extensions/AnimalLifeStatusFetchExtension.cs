using Core.Enumerations;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalLifeStatusFetchExtension
    {
        public static async Task<AnimalLifeStatus> FetchByEnumAsync(
            this IQueryable<AnimalLifeStatus> query, LifeStatus lifeStatus)
        {
            return await query
                .Where(als =>
                    als.LifeStatus.ToUpper()
                    == lifeStatus.ToString().ToUpper())
                .SingleAsync();
        }

        public static async Task<AnimalLifeStatus?> FetchByNameAsync(
            this IQueryable<AnimalLifeStatus> query, string lifeStatus)
        {
            return await query
                .Where(als =>
                    als.LifeStatus.ToUpper() 
                    == lifeStatus.ToUpper())
                .FirstOrDefaultAsync();
        }
    }
}
