using Microsoft.EntityFrameworkCore;
using WebScrapper.Models;

namespace WebScrapper.Context
{
    public class AdContext : DbContext
    {
        public DbSet<AdModel> AdModels { get; set; }
        public DbSet<FetchDate> FetchDates { get; set; }
        public DbSet<AdPriceModel> AdPriceModels { get; set; }

        //public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options)
        //{
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdModel>().HasKey(r => r.AdId);
            modelBuilder.Entity<FetchDate>().HasKey(f => f.Id);
            modelBuilder.Entity<AdPriceModel>().HasKey(a => a.AdPriceId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=real_estate; Username=admin; Password=admin; Pooling=true");
        }
    }
}
