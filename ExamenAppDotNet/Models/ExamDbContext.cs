using Microsoft.EntityFrameworkCore;

namespace ExamenAppDotNet.Models
{
    // DbContext = Unit of Work
    public class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions<ExamDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
            });
            builder.Entity<Package>(entity =>
            {
                entity.HasIndex(p => p.TrackingCode).IsUnique();
            });
        }

        // DbSet = Repository, O tabela din baza de 
        public DbSet<User> Users { get; set; }
        public DbSet<Package> Packages { get; set; }
    }
}
