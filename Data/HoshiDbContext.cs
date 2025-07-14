using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hoshi.Data
{
    public class HoshiDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public HoshiDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Loop through all foreign key relationships
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableForeignKey foreignKey in entityType.GetForeignKeys())
                {
                    // Set DeleteBehavior to NoAction for all foreign keys
                    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
                }
            }
        }
    }
}
