using Core.Enumerations;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AccountRoleFetchExtension
    {
        public static async Task<AccountRole> FetchByEnumAsync(
            this IQueryable<AccountRole> query, Role role)
        {
            return await query.FirstAsync(ar => ar.Id == ((int)role));
        }

        public static async Task<AccountRole?> FetchByNameAsync(
            this IQueryable<AccountRole> query, string role)
        {
            return await query
                .Where(ar =>
                    ar.Role.ToUpper()
                    == role.ToUpper())
                .FirstOrDefaultAsync();
        }
    }
}
