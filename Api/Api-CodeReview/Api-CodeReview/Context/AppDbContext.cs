using Api_CodeReview.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Api_CodeReview.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Repositorio> Repositorios { get; set; }
        public DbSet<Commit> Commits { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<SLA> SLAS { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"C:\Users\gabriella.santos\AppData\Roaming\Microsoft\UserSecrets\55096667-32d7-4a8c-bd60-0dfe502ba1a8\secrets.json", false, false)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DEV_CodeReview"));
        }
    }
}
