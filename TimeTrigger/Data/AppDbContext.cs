using Microsoft.Extensions.Configuration;


using TimeTrigger.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TimeTrigger.Data
{
    public class AppDbContext : IdentityDbContext<UserQ>
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }
      
        public DbSet<UserQ> userQ { get; set; }
      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["DefaultConnection"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

            //modelBuilder.Entity<UserQ>()
            //    .HasMany(u => u.UserCategories)
            //    .WithMany(c => c.CategoryUsers)
            //    .UsingEntity(j => j.ToTable("CategoryUser"));

            base.OnModelCreating(modelBuilder);

        }

    }
}
