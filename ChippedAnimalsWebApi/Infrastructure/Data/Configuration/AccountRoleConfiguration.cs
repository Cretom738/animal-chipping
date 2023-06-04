using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Enumerations;

namespace Infrastructure.Data.Configuration
{
    public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
    {
        public void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            builder.HasIndex(ar => ar.Role).IsUnique();
            string[] roleNames = Enum.GetNames(typeof(Role));
            for (int i = 0; i < roleNames.Length; i++)
            {
                builder.HasData(
                    new AccountRole
                    {
                        Id = i + 1,
                        Role = roleNames[i].ToUpper()
                    });
            }
        }
    }
}
