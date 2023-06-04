using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class AnimalVisitedLocationConfiguration : IEntityTypeConfiguration<AnimalVisitedLocation>
    {
        public void Configure(EntityTypeBuilder<AnimalVisitedLocation> builder)
        {
            builder.Property(avl => avl.VisitDateTime).HasColumnType("bigint");
        }
    }
}
