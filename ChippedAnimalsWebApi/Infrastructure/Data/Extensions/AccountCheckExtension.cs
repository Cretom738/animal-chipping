using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AccountCheckExtension
    {
        public static async Task<bool> DoesEmailExistsAsync(
            this IQueryable<Account> query, string email, string? currentAccountEmail = null)
        {
            return await query.Where(a => a.Email.ToLower() == email.ToLower()).AnyAsync();
        }

        public static async Task<bool> AreCorrectCredentialsAsync(
            this IQueryable<Account> query, string email, string password)
        {
            return await query.AnyAsync(a =>
                a.Email.ToLower() == email.ToLower()
                && a.Password == password);
        }

        public static async Task<bool> DoesExistsAsync(
            this IQueryable<Account> query, int? accountId)
        {
            return await query.AnyAsync(a => a.Id == accountId);
        }
    }
}
