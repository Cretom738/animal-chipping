using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class AnimalTypeConfiguration : IEntityTypeConfiguration<AnimalType>
    {
        public void Configure(EntityTypeBuilder<AnimalType> builder)
        {
            builder.HasIndex(t => t.Type).IsUnique();
        }
    }
}
