using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AccountFetchExtension
    {
        public static async Task<Account?> FetchByIdAsync(
            this IQueryable<Account> query, long? accountId)
        {
            return await query
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public static async Task<Account?> FetchByIdNoTrackingAsync(
            this IQueryable<Account> query, long? accountId)
        {
            return await query
                .Include(a => a.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public static async Task<Account?> FetchByEmailNoTrackingAsync(
            this IQueryable<Account> query, string email)
        {
            return await query
                .Include(a => a.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public static async Task<IList<Account>> FetchListAsync(
            this IQueryable<Account> query)
        {
            return await query
                .Include(a => a.Role)
                .ToListAsync();
        }
    }
}
