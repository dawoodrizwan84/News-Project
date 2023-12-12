using _23._1News.Data;
using _23._1News.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsLetterToQueue.Data
{
    public class QueDbContext
    {

        public class QueueDbContext : DbContext
        {
            private readonly IConfiguration _configuration;
            private readonly ApplicationDbContext _db;


            public QueueDbContext(DbContextOptions<ApplicationDbContext> options,
                IConfiguration configuration)
                : base(options)
            {
                _configuration = configuration;
                
            }

         
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(_configuration["DefaultConnection"]);

            }
        }
    }
}
