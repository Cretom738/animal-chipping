using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Enumerations;

namespace Infrastructure.Data.Configuration
{
    public class AnimalLifeStatusConfiguration : IEntityTypeConfiguration<AnimalLifeStatus>
    {
        public void Configure(EntityTypeBuilder<AnimalLifeStatus> builder)
        {
            builder.HasIndex(als => als.LifeStatus).IsUnique();
            string[] lifeStatusNames = Enum.GetNames(typeof(LifeStatus));
            for (int i = 0; i < lifeStatusNames.Length; i++)
            {
                builder.HasData(
                    new AnimalLifeStatus
                    {
                        Id = i + 1,
                        LifeStatus = lifeStatusNames[i].ToUpper()
                    });
            }
        }
    }
}
