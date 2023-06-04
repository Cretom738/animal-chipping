using Core.Enumerations;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions
{
    public static class AnimalGenderFetchExtension
    {
        public static async Task<AnimalGender> FetchByEnumAsync(
            this IQueryable<AnimalGender> query, Gender gender)
        {
            return await query
                .Where(ag =>
                    ag.Gender.ToUpper()
                    == gender.ToString().ToUpper())
                .SingleAsync();
        }

        public static async Task<AnimalGender?> FetchByNameAsync(
            this IQueryable<AnimalGender> query, string gender)
        {
            return await query
                .Where(ag =>
                    ag.Gender.ToUpper() 
                    == gender.ToUpper())
                .FirstOrDefaultAsync();
        }
    }
}
