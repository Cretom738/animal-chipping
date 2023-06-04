using Core.Models;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalFilterExtension
    {
        public static IQueryable<Animal> WhereStartDateTime(
            this IQueryable<Animal> query, DateTime? startDateTime)
        {
            return query.Where(a => a.ChippingDateTime >= startDateTime);
        }

        public static IQueryable<Animal> WhereEndDateTime(
            this IQueryable<Animal> query, DateTime? endDateTime)
        {
            return query.Where(a => a.ChippingDateTime <= endDateTime);
        }

        public static IQueryable<Animal> WhereChipperId(
            this IQueryable<Animal> query, int? chipperId)
        {
            return query.Where(a => a.ChipperId == chipperId);
        }

        public static IQueryable<Animal> WhereChippingLocationId(
            this IQueryable<Animal> query, long? chippingLocationId)
        {
            return query.Where(a => a.ChippingLocationId == chippingLocationId);
        }

        public static IQueryable<Animal> WhereLifeStatus(
            this IQueryable<Animal> query, string lifeStatus)
        {
            return query.Where(a =>
                a.LifeStatus.LifeStatus.ToUpper() == lifeStatus.ToUpper());
        }

        public static IQueryable<Animal> WhereGender(
            this IQueryable<Animal> query, string gender)
        {
            return query.Where(a =>
                a.Gender.Gender.ToUpper() == gender.ToUpper());
        }

        public static IQueryable<Animal> OrderById(this IQueryable<Animal> query)
        {
            return query.OrderBy(a => a.Id);
        }
    }
}
