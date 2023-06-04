using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AreaCheckExtension
    {
        public static async Task<bool> DoesNameExist(
            this IQueryable<Area> query, string areaName)
        {
            return await query
                .AnyAsync(a =>
                    a.Name.ToUpper()
                    == areaName.ToUpper());
        }
    }
}
