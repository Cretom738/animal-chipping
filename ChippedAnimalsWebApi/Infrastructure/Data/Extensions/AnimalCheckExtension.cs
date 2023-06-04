using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalCheckExtension
    {
        public static async Task<bool> DoesExistsAsync(
            this IQueryable<Animal> query, long? animalId)
        {
            return await query.AnyAsync(a => a.Id == animalId);
        }

        public static async Task<bool> IsAnyAssociatedWithAccountAsync(
            this IQueryable<Animal> query, int? accountId)
        {
            return await query.AnyAsync(a => a.ChipperId == accountId);
        }

        public static async Task<bool> IsAnyAssociatedWithLocationAsync(
            this IQueryable<Animal> query, long? locationId)
        {
            return await query.AnyAsync(a =>
                a.ChippingLocationId == locationId
                || a.VisitedLocations.Any(avl => avl.LocationId == locationId));
        }

        public static async Task<bool> IsAnyAssociatedWithAnimalTypeAsync(
            this IQueryable<Animal> query, long? typeId)
        {
            return await query.AnyAsync(a => a.Types.Any(at => at.Id == typeId));
        }
    }
}
