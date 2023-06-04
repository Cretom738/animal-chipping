using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AccountRoleCheckExtension
    {
        public static async Task<bool> DoesNameExistsAsync(
            this IQueryable<AccountRole> query, string roleName)
        {
            return await query.AnyAsync(ar =>
                ar.Role.ToUpper() == roleName.ToUpper());
        }
    }
}
