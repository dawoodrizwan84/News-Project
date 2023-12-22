using _23._1News.Models.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _23._1News.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }

        public DbSet<Category> Categories { get; set; }

        

        //public DbSet<Employee> Employees { get; set; }



        public DbSet<WeeklySubscriptionData> WeeklySubscriptionData { get; set; }
        public DbSet<HistoricalYahooData> HistoricalYahooData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeeklySubscriptionData>().HasNoKey();


            // Add other configurations as needed
            modelBuilder.Entity<HistoricalYahooData>()
                .HasNoKey();

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserCategories)
                .WithMany(c => c.CategoryUsers)
                .UsingEntity(j => j.ToTable("CategoryUser"));

            base.OnModelCreating(modelBuilder);

        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<HistoricalYahooData>()
        //        .HasNoKey();
        //}






    }
}