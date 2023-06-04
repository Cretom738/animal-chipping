using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data.Configuration.Extensions
{
    public static class ConfigurationExtension
    {
        public static void SetRestrictDeleteBehavior(this ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableForeignKey> foreignKeys = modelBuilder
                .Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership 
                    && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (IMutableForeignKey foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
