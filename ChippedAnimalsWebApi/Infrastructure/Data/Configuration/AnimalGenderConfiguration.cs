using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Enumerations;

namespace Infrastructure.Data.Configuration
{
    public class AnimalGenderConfiguration : IEntityTypeConfiguration<AnimalGender>
    {
        public void Configure(EntityTypeBuilder<AnimalGender> builder)
        {
            builder.HasIndex(ag => ag.Gender).IsUnique();
            string[] genderNames = Enum.GetNames(typeof(Gender));
            for (int i = 0; i < genderNames.Length; i++)
            {
                builder.HasData(
                    new AnimalGender { 
                        Id = i + 1, 
                        Gender = genderNames[i].ToUpper() 
                    });
            }
        }
    }
}
