using Api_CodeReview.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Api_CodeReview.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Repositorio> Repositorios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DEV_Code_Review"));
        }
    }
}
