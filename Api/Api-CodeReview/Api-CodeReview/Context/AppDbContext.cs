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
        public DbSet<FeedbackCommit> FeedbackCommits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            optionsBuilder.UseSqlServer(Service.CriptografiaService.Decrypt(configuration.GetConnectionString("DEV_CodeReview")));
        }
    }
}
