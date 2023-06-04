using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Enumerations;

namespace Infrastructure.Data.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasIndex(a => a.Email).IsUnique();
            builder.HasData(
                new Account
                {
                    Id = 1,
                    FirstName = "adminFirstName",
                    LastName = "adminLastName",
                    Email = "admin@simbirsoft.com",
                    Password = "qwerty123",
                    RoleId = (int)Role.Admin
                },
                new Account
                {
                    Id = 2,
                    FirstName = "chipperFirstName",
                    LastName = "chipperLastName",
                    Email = "chipper@simbirsoft.com",
                    Password = "qwerty123",
                    RoleId = (int)Role.Chipper
                },
                new Account
                {
                    Id = 3,
                    FirstName = "userFirstName",
                    LastName = "userLastName",
                    Email = "user@simbirsoft.com",
                    Password = "qwerty123",
                    RoleId = (int)Role.User
                });
        }
    }
}
