using Core.Models;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalVisitedLocationFilterExtension
    {
        public static IQueryable<AnimalVisitedLocation> WhereAnimalId(
            this IQueryable<AnimalVisitedLocation> query, long? animalId)
        {
            return query.Where(avl => avl.AnimalId == animalId);
        }

        public static IQueryable<AnimalVisitedLocation> WhereStartDateTime(
            this IQueryable<AnimalVisitedLocation> query, DateTime? startDateTime)
        {
            return query.Where(avl => avl.VisitDateTime >= startDateTime);
        }

        public static IQueryable<AnimalVisitedLocation> WhereEndDateTime(
            this IQueryable<AnimalVisitedLocation> query, DateTime? endDateTime)
        {
            return query.Where(avl => avl.VisitDateTime <= endDateTime);
        }

        public static IQueryable<AnimalVisitedLocation> OrderByVisitDateTime(
            this IQueryable<AnimalVisitedLocation> query)
        {
            return query.OrderBy(a => a.VisitDateTime);
        }
    }
}
