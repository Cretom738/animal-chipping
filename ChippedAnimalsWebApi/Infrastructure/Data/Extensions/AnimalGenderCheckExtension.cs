using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalGenderCheckExtension
    {
        public static async Task<bool> DoesNameExistsAsync(
            this IQueryable<AnimalGender> query, string genderName)
        {
            return await query.AnyAsync(ag =>
                ag.Gender.ToUpper() == genderName.ToUpper());
        }
    }
}
