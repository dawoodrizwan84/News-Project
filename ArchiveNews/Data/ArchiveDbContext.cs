
using _23._1News.Data;
using _23._1News.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveNews.Data
{
    public class ArchiveDbContext : ApplicationDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

      
        public ArchiveDbContext(DbContextOptions<ApplicationDbContext> options, 
            IConfiguration configuration) 
            : base(options)
        {
            _configuration = configuration;
        }
       
        public DbSet<Article> Articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["DefaultConnection"]);
 
        }

    }



 

}

