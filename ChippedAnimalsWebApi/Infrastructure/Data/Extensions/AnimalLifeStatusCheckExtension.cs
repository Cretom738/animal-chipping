using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalLifeStatusCheckExtension
    {
        public static async Task<bool> DoesNameExistsAsync(
            this IQueryable<AnimalLifeStatus> query, string lifeStatusName)
        {
            return await query.AnyAsync(als =>
                als.LifeStatus.ToUpper() == lifeStatusName.ToUpper());
        }
    }
}
