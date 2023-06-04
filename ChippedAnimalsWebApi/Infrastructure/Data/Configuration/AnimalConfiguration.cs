using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.Property(a => a.ChippingDateTime).HasColumnType("bigint");
            builder.Property(a => a.DeathDateTime).HasColumnType("bigint");
        }
    }
}
