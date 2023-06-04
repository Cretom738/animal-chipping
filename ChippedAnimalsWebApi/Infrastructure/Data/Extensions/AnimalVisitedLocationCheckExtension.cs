using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalVisitedLocationCheckExtension
    {
        public static async Task<bool> IsAnyAssociatedWithAnimal(
            this IQueryable<AnimalVisitedLocation> query, long? animalId)
        {
            return await query.AnyAsync(avl => avl.AnimalId == animalId);
        }
    }
}
