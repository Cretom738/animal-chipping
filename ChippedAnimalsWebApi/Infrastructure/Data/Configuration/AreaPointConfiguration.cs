using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configuration
{
    public class AreaPointConfiguration : IEntityTypeConfiguration<AreaPoint>
    {
        public void Configure(EntityTypeBuilder<AreaPoint> builder)
        {
            builder
                .HasOne(ap => ap.Area)
                .WithMany(a => a.AreaPoints)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
