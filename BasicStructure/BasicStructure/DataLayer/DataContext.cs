using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BasicStructure.DataLayer
{
    public class DataContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<int>>().HasData
            (
                new IdentityRole<int>() { Id = 1, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole<int>() { Id = 2, Name = "Manager", ConcurrencyStamp = "2", NormalizedName = "Manager" },
                new IdentityRole<int>() { Id = 3, Name = "Worker", ConcurrencyStamp = "3", NormalizedName = "Worker" }
            ); 
        }

        public DbSet<User> Users => Set<User>();
    }
}
