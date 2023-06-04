using Microsoft.EntityFrameworkCore;
using Core.Models;
using Infrastructure.Data.Configuration.Extensions;
using System.Reflection;

namespace Infrastructure.Data
{
    public class ChippedAnimalsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalGender> AnimalGenders { get; set; }
        public DbSet<AnimalLifeStatus> AnimalLifeStatuses { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaPoint> AreaPoints { get; set; }
        public DbSet<Location> Locations { get; set; }

        public ChippedAnimalsDbContext(DbContextOptions<ChippedAnimalsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetRestrictDeleteBehavior();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}