using Brewery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brewery.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<BreweryEntity> Breweries { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<BreweryEntity>(entity =>
        //    {
        //        entity.HasKey(b => b.id);
        //        entity.Property(b => b.name).IsRequired().HasMaxLength(200);
        //        entity.Property(b => b.city).HasMaxLength(100);
        //    });
        //}
    }
}
