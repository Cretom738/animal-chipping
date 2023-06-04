using Core.Models;

namespace Infrastructure.Data.Extensions
{
    public static class AccountFilterExtension
    {
        public static IQueryable<Account> WhereFirstName(
            this IQueryable<Account> query, string firstName)
        {
            return query.Where(a => a.FirstName.ToLower().Contains(firstName.ToLower()));
        }

        public static IQueryable<Account> WhereLastName(
            this IQueryable<Account> query, string lastName)
        {
            return query.Where(a => a.LastName.ToLower().Contains(lastName.ToLower()));
        }

        public static IQueryable<Account> WhereEmail(
            this IQueryable<Account> query, string email)
        {
            return query.Where(a => a.Email.ToLower().Contains(email.ToLower()));
        }

        public static IQueryable<Account> OrderById(this IQueryable<Account> query)
        {
            return query.OrderBy(a => a.Id);
        }
    }
}
