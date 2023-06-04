using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalTypeCheckExtension
    {
        public static async Task<bool> DoesNameExistsAsync(
            this IQueryable<AnimalType> query, string typeName)
        {
            return await query.AnyAsync(at => at.Type.ToLower() == typeName.ToLower());
        }

        public static async Task<IEnumerable<long>> SelectNonExistentIdsAsync(
            this IQueryable<AnimalType> query, ICollection<long> typeIds)
        {
            return typeIds
                .Except(await query
                    .Where(at => typeIds.Contains(at.Id))
                    .Select(at => at.Id)
                    .ToListAsync());
        }
    }
}
